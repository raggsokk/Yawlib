#region License
//
// WmiConnection.cs
// 
// The MIT License (MIT)
//
// Copyright (c) 2015 Jarle Hansen
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE. 
//
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using System.Management;
using System.Diagnostics;
using System.Threading;

using Yawlib.Magic;

namespace Yawlib
{
    /// <summary>
    /// Represents a wmi scope to a local or remote server.
    /// (Could just have been extension methods to ManagementScope thou.)
    /// </summary>
    [DebuggerDisplay("{Scope.Path.ToString()}")]
    public class WmiConnection
    {
        public ManagementScope Scope { get; protected set; }

        #region Constructors

        /// <summary>
        /// Creates a connection to local machine with default settings.
        /// aka '\\.\ROOT\cimv2'
        /// or to '\\.\ROOT\{SubNamespace}
        /// <param name="SubNamespace">'\\.\ROOT\{SubNamespace}</param>
        /// </summary>
        public WmiConnection(string SubNamespace = null)
        {
            if(!string.IsNullOrWhiteSpace(SubNamespace))
            {
                Scope = new ManagementScope(string.Format(
                    "\\\\.\\ROOT\\{0}", ValidateCorrectSubNamespace(SubNamespace)));
            }
            else
                Scope = new ManagementScope();
        }

        /// <summary>
        /// Enables user created path and options which hopefully works better than our provided constructors...
        /// </summary>
        /// <param name="path"></param>
        /// <param name="options"></param>
        public WmiConnection(ManagementPath path, ConnectionOptions options)
        {
            Scope = new ManagementScope(path, options);
        }

        /// <summary>
        /// Helper function for creating remote wmi connections.
        /// </summary>
        /// <param name="IpOrHostname">Ip or hostname to connect to.</param>
        /// <param name="username">Username to run as.</param>
        /// <param name="password">Password of user.</param>
        /// <param name="SubNamespace">Which subnamespace to connect to or default to 'cimv2'</param>
        /// <returns></returns>
        public static WmiConnection CreateRemoteConnection(string IpOrHostname, string username, string password, string SubNamespace = null)
        {
            var domain = string.Empty;
            ConnectionOptions options;

            if (!string.IsNullOrWhiteSpace(username))
            {
                var slash = username.IndexOf('\'');
                if (slash != -1)
                {
                    domain = username.Substring(0, slash);
                    username = username.Substring(slash + 1);
                }
                else
                {
                    var at = username.IndexOf(('@'));
                    if (at != -1)
                    {
                        domain = username.Substring(0, at);
                        username = username.Substring(at + 1);
                    }
                }
                options = new ConnectionOptions()
                {
                    Username = !string.IsNullOrWhiteSpace(username) ? username : null,
                    Password = !string.IsNullOrWhiteSpace(password) ? password : null,
                };
                if (!string.IsNullOrWhiteSpace(domain))
                {
                    options.Authority = string.Format("ntdlmdomain:{0}", domain);
                }
            }
            else
                options = new ConnectionOptions();
            
            var path = string.Format("\\\\{0}\\ROOT\\{1}", IpOrHostname, ValidateCorrectSubNamespace(SubNamespace));
            
            return new WmiConnection(new ManagementPath(path), options);            
        }

        /// <summary>
        /// Private validate user input namespace utility.
        /// </summary>
        /// <param name="SubNamespace"></param>
        /// <returns></returns>
        private static string ValidateCorrectSubNamespace(string SubNamespace = null)
        {
            if (string.IsNullOrWhiteSpace(SubNamespace))
                return "cimv2";
            else if (SubNamespace.Length > 5 && SubNamespace.StartsWith("ROOT", StringComparison.InvariantCultureIgnoreCase))
                return SubNamespace.Substring(5);

            return SubNamespace;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Are we connected?
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return Scope.IsConnected;
            }
        }

        /// <summary>
        /// The WMI namespace we are connected to.
        /// aka \\{server}\ROOT\{SubNamespace}
        /// </summary>
        public string SubNamespace
        {
            get
            {
                if (Scope.Path.NamespacePath.Length > 5)
                    return Scope.Path.NamespacePath.Substring(5);
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// The machine we are connected to or '.' if local machine.
        /// </summary>
        public string Server
        {
            get
            {
                return Scope.Path.Server;
            }
        }

        #endregion

        #region Various Functions

        /// <summary>
        /// Connects the scope with ctor spesified args.
        /// This enables the user to modify the scope settings before connecting.
        /// TODO: why is this a separate function call instead of ctor internal?
        /// </summary>
        public void Connect()
        {
            //TODO: why is this a separate function call instead of ctor internal?
            this.Scope.Connect();
        }

        /// <summary>
        /// Simple synchronous Query without any parsing.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<ManagementBaseObject> Query(ObjectQuery query)
        {
            //list to hold our result.
            var result = new List<ManagementBaseObject>();

            using (var searcher = new ManagementObjectSearcher(this.Scope, query))
            {
                // query synchronous.
                var data = searcher.Get();

                // convert managementobjectcollection to list<T>
                foreach (var item in data)
                    result.Add(item);
            }

            //return parsed data.
            return result;
        }

        /// <summary>
        /// Simple Asynchronous Query without any parsing.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<ManagementBaseObject>> QueryAsync(ObjectQuery query, CancellationToken cancellationToken = default(CancellationToken))
        {
            //TODO: Create an queryasync with action callback instead so that we can convert data while it streams in.
            var result = new List<ManagementBaseObject>();
            var tsc = new TaskCompletionSource<List<ManagementBaseObject>>();
            var watcher = new ManagementOperationObserver();

            // setting up the watcher event handlers

            //Add data input handler.
            watcher.ObjectReady += (sender, args) =>
            {
                // basic cancellation support. Doesn't handle network timeouts (aka you have to wait...)
                if (cancellationToken.IsCancellationRequested)
                    tsc.TrySetCanceled(cancellationToken);

                // try to gracefully handle possible exceptions in parser code.
                try
                {
                    result.Add(args.NewObject);
                }
                catch (Exception e)
                {
                    tsc.SetException(e);
                }
            };

            //Add data completed handler.
            watcher.Completed += (obj, e) =>
            {
                //TODO: handle e.Status??
                tsc.TrySetResult(result);
            };

            // Create a searcher using our scope.
            using (var searcher = new ManagementObjectSearcher(this.Scope, query))
            {
                // start doing work async.
                searcher.Get(watcher);

                return await tsc.Task;
            }
        }

        /// <summary>
        /// Runs a simple wql string query and returns a list of managementbaseobjects.
        /// </summary>
        /// <param name="wqlQuery"></param>
        /// <returns></returns>
        public List<ManagementBaseObject> Query(string wqlQuery)
        {
            var query = new SelectQuery(wqlQuery);

            return Query(query);
        }

        /// <summary>
        /// Runs a simple wql string query async and returns a list of managementbaseobjects.
        /// </summary>
        /// <param name="wqlQuery"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<ManagementBaseObject>> QueryAsync(string wqlQuery, CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = new SelectQuery(wqlQuery);

            return await QueryAsync(query, cancellationToken);
        }

        #endregion

        #region Custom Parsing

        /// <summary>
        /// Common utility function for running parsing on all managementbaseobjects in a list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="Parser"></param>
        /// <returns></returns>
        private List<T> Convert<T>(List<ManagementBaseObject> list, Func<ManagementBaseObject, T> Parser)
        {
            var result = new List<T>();

            foreach (var item in list)
                result.Add(Parser(item));

            return result;
        }

        /// <summary>
        /// Runs a user create ObjectQuery object with custom parser.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="Parser"></param>
        /// <returns></returns>
        public List<T> Query<T>(ObjectQuery query, Func<ManagementBaseObject, T> Parser)
        {
            var list = Query(query);

            return Convert<T>(list, Parser);
        }

        /// <summary>
        /// Runs a user created objectquery async with custom parser.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="Parser"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<T>> QueryAsync<T>(ObjectQuery query, Func<ManagementBaseObject, T> Parser, CancellationToken cancellationToken = default(CancellationToken))
        {
            var list = await QueryAsync(query);

            return Convert<T>(list, Parser);
        }

        /// <summary>
        /// Returns all instances of a wmi class and parsing them with custom parser.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Parser"></param>
        /// <returns></returns>
        public List<T> Get<T>(Func<ManagementBaseObject, T> Parser)
        {
            //TODO: find another way to get wmiclassname which is faster than fullblown reflection generation.
            var t = typeof(T);
            var myType = Reflection.Instance.TryGetMyType(t.FullName, t);
            var query = new SelectQuery(myType.CreateSelectAll());

            var list = Query(query);

            return Convert<T>(list, Parser);
        }

        /// <summary>
        /// Returns all instances async of a wmi class and parses them with custom parser.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Parser"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<T>> GetAsync<T>(Func<ManagementBaseObject, T> Parser, CancellationToken cancellationToken = default(CancellationToken))
        {
            //TODO: find another way to get wmiclassname which is faster than fullblown reflection generation.
            var t = typeof(T);
            var myType = Reflection.Instance.TryGetMyType(t.FullName, t);
            var query = new SelectQuery(myType.CreateSelectAll());

            var list = await QueryAsync(query, cancellationToken);

            return Convert<T>(list, Parser);
        }

        #endregion

        #region Magic Parsing

        /// <summary>
        /// Common magic parser helper function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="myType"></param>
        /// <returns></returns>
        private List<T> Convert<T>(List<ManagementBaseObject> list, clsMyType myType = null)
        {
            if(myType == null)
            {
                var t = typeof(T);
                myType = Reflection.Instance.TryGetMyType(t.FullName, t);
            }            
            //var myType = Reflection.Instance.TryGetMyType(t.FullName, t);

            var result = new List<T>();
            MyConvert.Convert(list, myType, result);

            return result;
        }

        /// <summary>
        /// Runs an usercreated ObjectQuery and parses them using magic into objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> Query<T>(ObjectQuery query)
        {
            var list = Query(query);

            return Convert<T>(list);
        }

        /// <summary>
        /// Runs an usercreated ObjectQuery async and parses them using magic into objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<T>> QueryAsync<T>(ObjectQuery query, CancellationToken cancellationToken = default(CancellationToken))
        {
            var list = await QueryAsync(query, cancellationToken);

            return Convert<T>(list);
        }

        /// <summary>
        /// Returns all instances of a wmi class and parses them using magic into objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> Get<T>()
        {
            var t = typeof(T);
            var myType = Reflection.Instance.TryGetMyType(t.FullName, t);
            var query = new SelectQuery(myType.CreateSelectAll());

            var list = Query(query);

            return Convert<T>(list, myType);
        }

        /// <summary>
        /// Returns async all instances of a wmi class and parses them using magic into objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<T>> GetAsync<T>(CancellationToken cancellationToken = default(CancellationToken))
        {
            var t = typeof(T);
            var myType = Reflection.Instance.TryGetMyType(t.FullName, t);
            var query = new SelectQuery(myType.CreateSelectAll());

            var list = await QueryAsync(query);

            return Convert<T>(list, myType);
        }

        #endregion
    }
}

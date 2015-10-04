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

namespace yawlib
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
        /// aka '\\.\root\cimv2'
        /// </summary>
        public WmiConnection()
        {
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
        /// Connects to remote ip or hostname, with provided credentials.
        /// TODO: Handle other wmi paths than root/cimv2 such as root/intelNCS2
        /// </summary>
        /// <param name="IpOrHostname"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public WmiConnection(string IpOrHostname, string username, string password)
        {
            var domain = string.Empty;
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

            var options = new ConnectionOptions()
            {
                Username = username,
                Password = password,
            };
            if (!string.IsNullOrWhiteSpace(domain))
            {
                options.Authority = string.Format("ntdlmdomain:{0}", domain);
            }

            var path = string.Format("\\\\{0}\\root\\cimv2", IpOrHostname);

            this.Scope = new ManagementScope(path, options);
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

        #endregion

        #region Wmi Query

        /// <summary>
        /// Executes a WMI query which will get parsed by specified parser.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="Parse"></param>
        /// <returns></returns>
        public List<T> Query<T>(ObjectQuery query, Func<ManagementBaseObject, T> Parse)
        {
            // Validate Parse is not empty.
            if (Parse == null)
                throw new ArgumentNullException(nameof(Parse), "Parser delegate cant be null");

            //list to hold our result.
            var list = new List<T>();

            using (var searcher = new ManagementObjectSearcher(this.Scope, query))
            {
                // query synchronous.
                var data = searcher.Get();

                // enumerate managementbaseobjects.
                foreach (var item in data)
                {
                    // parse and add to list.
                    list.Add(Parse(item));
                }
            }

            //return parsed data.
            return list;
        }

        /// <summary>
        /// Executes a WMI Query asynchronous which will get parsed on return by specified parser.
        /// NOTE: works best for workgroup computers and interdomain machines due to security settings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="Parse"></param>
        /// <returns></returns>
        public async Task<List<T>> QueryAsync<T>(ObjectQuery query, Func<ManagementBaseObject, T> Parse, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Validate Parse is not empty.
            if (Parse == null)
                throw new ArgumentNullException(nameof(Parse), "Parser delegate cant be null");

            // set up our async helper.
            var tsc = new TaskCompletionSource<List<T>>();
            var list = new List<T>();

            // create watcher control
            var watcher = new ManagementOperationObserver();

            //Add data input handler.
            watcher.ObjectReady += (sender, args) =>
            {
                // basic cancellation support. Doesn't handle network timeouts (aka you have to wait...)
                if (cancellationToken.IsCancellationRequested)
                    tsc.TrySetCanceled(cancellationToken);
                
                // try to gracefully handle possible exceptions in parser code.
                try
                {
                    var item = Parse(args.NewObject);
                    list.Add(item);
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
                tsc.TrySetResult(list);
            };

            // Create a searcher using our scope.
            using (var searcher = new ManagementObjectSearcher(this.Scope, query))
            {
                // start doing work async.
                searcher.Get(watcher);
                
                return await tsc.Task;
            }

            // await async task now so we can parse it.
            //await tsc.Task;

            //return list;
        }

        /// <summary>
        /// Exequtes a wmi query which get parsed by T.IWmiParsable.Parse
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> Query<T>(ObjectQuery query) where T : IWmiParseable
        {
            var t = default(T);

            return Query<T>(query, (x) => 
                { return (T)t.Parse(x); }
            );
        }

        /// <summary>
        /// Executes a WMI Query asynchronous which will get parsed on return by implemented IWmiParsable.Parse.
        /// NOTE: works best for workgroup computers and interdomain machines due to security settings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<T>> QueryAsync<T>(ObjectQuery query, CancellationToken cancellationToken = default(CancellationToken)) where T : IWmiParseable
        {
            var t = default(T);

            return await QueryAsync<T>(query, (x) =>
            {
                return (T)t.Parse(x);
            }, cancellationToken);

        }

        #endregion

        #region Get All Functions

        //TODO: Move this function to reflection util class.
        //TODO: Maybe cache the result?
        private static SelectQuery CreateSelectAll(Type t)
        {
            var wminame = string.Empty;

            var attribWmiClassName = t.GetCustomAttribute<WmiClassNameAttribute>();

            if (attribWmiClassName != null)
                wminame = attribWmiClassName.WmiClassName;
            else
                wminame = t.Name; // fall back to use .net class name

            var wql = string.Format("SELECT * FROM {0}", wminame);

            return new SelectQuery(wql);
        }


        /// <summary>
        /// Retrives all instances of specified wmiclass from wmi connection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Parse"></param>
        /// <returns></returns>
        public List<T> Get<T>(Func<ManagementBaseObject, T> Parse)
        {
            var query = CreateSelectAll(typeof(T));

            return Query<T>(query, Parse);
        }

        /// <summary>
        /// Retrives all instances of specified wmiclass from wmi connection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Parse"></param>
        /// <returns></returns>
        public async Task<List<T>> GetAsync<T>(Func<ManagementBaseObject, T> Parse)
        {
            var query = CreateSelectAll(typeof(T));

            return await QueryAsync<T>(query, Parse);
        }

        /// <summary>
        /// Retrives all instances of specified wmiclass from wmi connection.
        /// Requires classes to implement IWmiParsable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> Get<T>() where T : IWmiParseable
        {
            //TODO: use type system here instead.
            var t = default(T);
            return Get<T>((x) => { return (T)t.Parse(x); });

            //var query = CreateSelectAll(typeof(T));           
            //return Query<T>(query, });
        }

        /// <summary>
        /// Retrives all instances of specified wmiclass from wmi connection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<List<T>> GetAsync<T>() where T : IWmiParseable
        {
            //TODO: use type system here instead.
            var t = default(T);
            return await GetAsync<T>((x) => { return (T)t.Parse(x); });
        }


        #endregion
    }
}

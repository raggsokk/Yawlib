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
        public async Task<List<ManagementBaseObject>> Query(ObjectQuery query, CancellationToken cancellationToken = default(CancellationToken))
        {
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

        #endregion
    }
}

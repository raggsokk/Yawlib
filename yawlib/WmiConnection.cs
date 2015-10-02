using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Management;
using System.Diagnostics;

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

        public bool IsConnected
        {
            get
            {
                return Scope.IsConnected;
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Connects the scope with ctor spesified args.
        /// TODO: why is this a separate function call instead of ctor internal?
        /// </summary>
        public void Connect()
        {
            //TODO: why is this a separate function call instead of ctor internal?
            this.Scope.Connect();
        }

        /// <summary>
        /// Executes a WMI query which will get parsed by specified parser.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="Parse"></param>
        /// <returns></returns>
        public List<T> Execute<T>(ObjectQuery query, Func<ManagementBaseObject, T> Parse)
        {
            // Validate Parse is not empty.
            if (Parse == null)
                throw new ArgumentNullException(nameof(Parse), "Parser delegate cant be null");

            // Create a searcher using our scope.
            var searcher = new ManagementObjectSearcher(this.Scope, query);

            // execute synchronous query.
            var data = searcher.Get();

            //list to hold our result.
            var list = new List<T>();

            // enumerate managementbaseobjects.
            foreach (var item in data)
            {
                // parse and add to list.
                list.Add(Parse(item));
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
        public async Task<List<T>> ExecuteAsync<T>(ObjectQuery query, Func<ManagementBaseObject, T> Parse)
        {
            // Validate Parse is not empty.
            if (Parse == null)
                throw new ArgumentNullException(nameof(Parse), "Parser delegate cant be null");

            // Create a searcher using our scope.
            var searcher = new ManagementObjectSearcher(this.Scope, query);

            // set up our async helper.
            var tsc = new TaskCompletionSource<List<T>>();
            var list = new List<T>();

            // create watcher control
            var watcher = new ManagementOperationObserver();

            //Add data input handler.
            watcher.ObjectReady += (sender, args) =>
            {
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

            // start doing work async.
            searcher.Get(watcher);

            return await tsc.Task;

            // await async task now so we can parse it.
            //await tsc.Task;

            //return list;
        }


        #endregion
    }
}

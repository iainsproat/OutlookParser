using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Redemption;

namespace OutlookParser
{
    public class PstFile : IDisposable
    {
        private bool _disposed;
        private bool _loggedOn;
        private RDOSession session;
        private RDOPstStore _store;

        public PstFile(string pathToFile)
        {
            if (string.IsNullOrWhiteSpace(pathToFile))
            {
                throw new ArgumentException("A fully formed and valid path should be provided.", "pathToFile");
            }

            if (!System.IO.File.Exists(pathToFile))
            {
                throw new ArgumentException("A fully formed and valid path to an existing .pst file should be provided.", "pathToFile");
            }

            if (!pathToFile.ToLowerInvariant().EndsWith(".pst"))
            {
                throw new ArgumentException("A path ending with '.pst' is expected");
            }

            this._loggedOn = false;
            this.Path = pathToFile;
        }

        public string Path
        {
            get;
            private set;
        }

        #region IDisposable
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.session != null)
                {
                    this.session.Logoff();
                    this._loggedOn = false;
                }
            }

            //release resources
            this.session = null;
            this._store = null;

            this._disposed = true;
        }
        #endregion

        public RDOPstStore Store
        {
            get
            {
                if(!_loggedOn)
                {
                    this.Logon();
                }

                return this._store;
            }
        }

        private void Logon()
        {
            this.session = new Redemption.RDOSession();
//            this.session.LogonPstStore(this.Path);
            this._loggedOn = true;
        }
    }
}

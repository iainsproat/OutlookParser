using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Outlook = Microsoft.Office.Interop.Outlook;

namespace OutlookParser
{
    public class PstFile : IDisposable
    {
        private bool _disposed;
        private bool _loggedOn;
        private Outlook.Application _application;
        private Outlook.Store _store;

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
                if (this._application != null)
                {
                    this.Logoff();
                }
            }

            //release resources
            this._application = null;
            this._store = null;

            this._disposed = true;
        }
        #endregion

        public Outlook.Store Store
        {
            get
            {
                if(!_loggedOn || this._application == null || this._store == null)
                {
                    this.Logon();
                }

                return this._store;
            }
        }

        public Outlook.Folder RootFolder
        {
            get
            {
                return this.Store.GetRootFolder() as Outlook.Folder;
            }
        }

        public IEnumerable<Outlook.MailItem> AllItems
        {
            get
            {
                IList<Outlook.MailItem> mailItems = new List<Outlook.MailItem>();
                this.ExtractItems(mailItems, this.RootFolder);
                return mailItems;
            }
        }

        private void ExtractItems(IList<Outlook.MailItem> mailItems, Outlook.Folder folder)
        {
            if (mailItems == null)
            {
                throw new ArgumentNullException("mailItems");
            }

            Outlook.Items items = folder.Items;

            int itemcount = items.Count;

            foreach (object item in items)
            {
                if (item is Outlook.MailItem)
                {
                    Outlook.MailItem mailItem = item as Outlook.MailItem;
                    mailItems.Add(mailItem);
                }
            }

            foreach (Outlook.Folder subfolder in folder.Folders)
            {
                ExtractItems(mailItems, subfolder);
            }
        }

        private void Logon()
        {
            this._application = new Outlook.Application();
            this._application.Session.AddStore(this.Path);
            Outlook.Stores stores = this._application.Session.Stores;
            foreach (Outlook.Store store in stores)
            {
                if (store.FilePath == this.Path)
                {
                    this._store = store;
                }
            }

            this._loggedOn = true;
        }

        public void Logoff()
        {
            this._application.Session.RemoveStore(this.RootFolder); //FIXME what happens if this._store is null, or the store does not have a valid root folder?  Is a try/catch required?
            this._loggedOn = false;
        }
    }
}

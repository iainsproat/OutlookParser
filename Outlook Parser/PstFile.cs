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
        private bool _connected;
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

            this._connected = false;
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
                    this.UnloadAndDisconnect();
                }
            }

            //release resources
            this._application = null;
            this._store = null;

            this._disposed = true;
        }
        #endregion

        public Outlook.Store Store //FIXME remove external reference to Outlook namespace
        {
            get
            {
                if (!_connected || this._application == null || this._store == null)
                {
                    this.ConnectAndLoad();
                }

                return this._store;
            }
        }

        public Outlook.Folder RootFolder //FIXME remove external reference to Outlook namespace
        {
            get
            {
                return this.Store.GetRootFolder() as Outlook.Folder;
            }
        }

        public IEnumerable<Email> AllItems
        {
            get
            {
                IList<Email> mailItems = new List<Email>();
                this.ExtractItems(mailItems, this.RootFolder);
                return mailItems;
            }
        }

        private void ExtractItems(IList<Email> mailItems, Outlook.Folder folder)
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

                    mailItems.Add(MapToEmail(mailItem));
                }
            }

            foreach (Outlook.Folder subfolder in folder.Folders)
            {
                ExtractItems(mailItems, subfolder);
            }
        }

        protected Email MapToEmail(Outlook.MailItem mailItem)
        {
            var email = new Email();
            email.Subject = mailItem.Subject;
            email.ReceivedTime = mailItem.ReceivedTime;
            email.Sender = this.GetSenderSMTPAddress(mailItem);
            email.Recipients = new List<string>(mailItem.Recipients.Count);
            foreach (Outlook.Recipient recipient in mailItem.Recipients)
            {
                email.Recipients.Add(this.GetRecipientSMTPAddress(recipient));
            }

            email.Attachments = mailItem.Attachments.Count;
            return email;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        /// <remarks>Code from http://msdn.microsoft.com/en-us/library/ff184624(v=office.14).aspx </remarks>
        protected string GetSenderSMTPAddress(Outlook.MailItem mail)
        {
            if (mail == null)
            {
                throw new ArgumentNullException("mail");
            }

            if (mail.SenderEmailType != "EX")
            {
                return mail.SenderEmailAddress;
            }

            Outlook.AddressEntry sender = mail.Sender;
            if (sender == null)
            {
                return string.Empty;
            }

            if (sender.AddressEntryUserType == Outlook.OlAddressEntryUserType.olExchangeUserAddressEntry
                || sender.AddressEntryUserType == Outlook.OlAddressEntryUserType.olExchangeRemoteUserAddressEntry)
            {
                Outlook.ExchangeUser exchUser = sender.GetExchangeUser();
                if (exchUser != null)
                {
                    return exchUser.PrimarySmtpAddress;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return this.GetSMTPAddressUsingPropertyAccessor(sender.PropertyAccessor);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recip"></param>
        /// <returns></returns>
        /// <remarks>Code from http://msdn.microsoft.com/en-us/library/ff184647(v=office.14).aspx </remarks>
        private string GetRecipientSMTPAddress(Outlook.Recipient recip)
        {
            return this.GetSMTPAddressUsingPropertyAccessor(recip.PropertyAccessor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pa"></param>
        /// <returns></returns>
        /// <remarks>Code from http://msdn.microsoft.com/en-us/library/ff184647(v=office.14).aspx </remarks>
        private string GetSMTPAddressUsingPropertyAccessor(Outlook.PropertyAccessor pa)
        {
            const string PR_SMTP_ADDRESS =
                "http://schemas.microsoft.com/mapi/proptag/0x39FE001E";
            return pa.GetProperty(PR_SMTP_ADDRESS).ToString();
        }

        private void ConnectAndLoad()
        {
            //TODO use a new/temporary profile and other security measures
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

            this._connected = true;
        }

        public void UnloadAndDisconnect()
        {
            this._application.Session.RemoveStore(this.RootFolder); //FIXME what happens if this._store is null, or the store does not have a valid root folder?  Is a try/catch required?
            this._connected = false;
        }
    }
}

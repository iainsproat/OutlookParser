using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OutlookParser;
using OutlookParserConsoleApp.Models;
using OutlookParserConsoleApp.Controllers;

namespace OutlookParserConsoleApp.Views
{
    public class AddAdditionalDataView : IView<AddAdditionalDataController, AddAdditionalDataModel>, IDisposable
    {
        private bool _disposed;
        protected AddAdditionalDataModel mdl;
        protected AddAdditionalDataController contrlr;

        public AddAdditionalDataView(AddAdditionalDataModel model)
        {
            this._disposed = false;
            this.mdl = model;
            this.Register(model);
        }

        #region MVC boilerplate
        public AddAdditionalDataModel Model
        {
            get
            {
                return this.mdl;
            }
        }

        public AddAdditionalDataController Controller
        {
            get
            {
                if (this.contrlr == null)
                {
                    this.contrlr = new AddAdditionalDataController(this.Model);
                }

                return this.contrlr;
            }
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("Controller cannot be null.");
                }

                if (this.contrlr != null)
                {
                    throw new InvalidOperationException("A controller for this view has already been set.");
                }

                this.contrlr = value;
            }
        }

        public void Register(AddAdditionalDataModel model)
        {
            model.PathToPstFilesUpdated += this.DisplayUpdatedPathToPstFiles;
            model.FoundEmails += this.DisplayFoundEmails;
        }

        public void Release(AddAdditionalDataModel model)
        {
            model.PathToPstFilesUpdated -= this.DisplayUpdatedPathToPstFiles;
            model.FoundEmails -= this.DisplayFoundEmails;
        }

        #region IDisposal
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
                if (this.Model != null)
                {
                    this.Release(this.Model);
                }
            }

            //free resources
            this.mdl = null;
            this.contrlr = null;

            this._disposed = true;
        }
        #endregion
        #endregion

        public void Show()
        {
            Console.WriteLine("Please type the path to a folder containing .pst files to import:");
            string pathToPstFiles = Console.ReadLine();
            if (MainMenuView.IsUserTryingToExit(pathToPstFiles))
            {
                Console.WriteLine("Returning to the main menu.");
                return;
            }

            try
            {
                this.Controller.PathToPstFilesEntered(pathToPstFiles);
            }
            catch (PstPathException ppe)
            {
                Console.WriteLine("Unfortunately we could not work with the provided path of : {0}", ppe.PstPath);
                Console.WriteLine(ppe.Message);
                Console.WriteLine("Please try again, or type 'e' to return to the main menu.");
                this.Show();
            }
        }

        protected void DisplayUpdatedPathToPstFiles(string path)
        {
            Console.WriteLine("You requested that the following path is searched for .Pst files: '{0}'", path);
        }

        protected void DisplayFoundEmails(IEnumerable<Email> emails)
        {
            int count = 0;
            foreach (Email email in emails)
            {
                Console.WriteLine("Email {0}", count);
                Console.WriteLine("\tSubject  : {0}", email.Subject);
                Console.WriteLine("\tReceived : {0}", email.ReceivedTime);
                count++;
            }
        }
    }
}

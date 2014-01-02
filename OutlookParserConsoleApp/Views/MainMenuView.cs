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
    public class MainMenuView : IView<MainMenuController, MainMenuModel>, IDisposable
    {
        private bool _disposed;
        private const string exit = "e";
        private const string exit2 = "exit";
        protected MainMenuModel mdl;
        protected MainMenuController contrlr;

        public MainMenuView(MainMenuModel model)
        {
            this._disposed = false;
            this.mdl = model;
            this.Register(model);
        }

        public MainMenuModel Model
        {
            get
            {
                return this.mdl;
            }
        }

        public MainMenuController Controller
        {
            get
            {
                if (this.contrlr == null)
                {
                    this.contrlr = new MainMenuController(this.Model);
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

        public void Register(MainMenuModel model)
        {
            model.PathToPstFilesUpdated += this.DisplayUpdatedPathToPstFiles;
            model.FoundEmails += this.DisplayFoundEmails;
        }

        public void Release(MainMenuModel model)
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

        public void Run()
        {
            Console.WriteLine("This application currently only reads Pst files.");
            this.GetPathToPstFiles();
        }

        public void GetPathToPstFiles()
        {
            Console.WriteLine("Please provide a path to a folder containing .pst files:");
            string pathToPstFiles = Console.ReadLine();
            try
            {
                this.Controller.PathToPstFilesEntered(pathToPstFiles);
            }
            catch(PstPathException ppe)
            {
                if (IsUserTryingToExit(ppe))
                {
                    Console.WriteLine("User has abandoned attempt to locate Pst files.");
                    return;
                }

                Console.WriteLine("Unfortunately we could not work with the provided path of : {0}", ppe.PstPath);
                Console.WriteLine(ppe.Message);
                Console.WriteLine("Please try again, or press 'e' to exit.");
                this.GetPathToPstFiles();
            }
        }

        private bool IsUserTryingToExit(PstPathException ppe)
        {
            return ppe.PstPath == exit || ppe.PstPath == exit2;
        }

        public void DisplayUpdatedPathToPstFiles(string path)
        {
            Console.WriteLine("You requested that the following path is searched for .Pst files: '{0}'", path);
        }

        public void DisplayFoundEmails(IEnumerable<Email> emails)
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

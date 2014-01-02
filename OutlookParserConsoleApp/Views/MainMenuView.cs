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
        protected MainMenuModel mdl;
        protected MainMenuController contrlr;

        public MainMenuView(MainMenuModel model)
        {
            this._disposed = false;
            this.mdl = model;
            this.Register(model);
        }

        #region MVC boilerplate
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
            // empty
        }

        public void Release(MainMenuModel model)
        {
            // empty
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

        public void Run()
        {
            Console.WriteLine("This application currently only reads Pst files.");
            while (true)
            {
                this.DisplayMainMenuWelcome();
                this.DisplayMainMenuOptions();
                if(!this.RespondToMainMenuOptions())
                {
                    break; // the user wishes to exit.
                }
            }

            Console.WriteLine("Exiting application.");
        }

        public void DisplayMainMenuWelcome()
        {
            Console.WriteLine("Welcome to the main menu of the Email parsing and visualisation application.");
            Console.WriteLine("{0} emails already exist in the database.", this.Model.NumberOfExistingEmails());
        }

        public void DisplayMainMenuOptions()
        {
            Console.WriteLine("Press one of the following keys to choose from the below options:");
            Console.WriteLine("1 = Add additional data.");
            Console.WriteLine("2 = Work with the existing data.");
            Console.WriteLine("e = Exit the application.");
        }

        public bool RespondToMainMenuOptions()
        {
            string userInput = Console.ReadLine();
            switch(userInput.ToLowerInvariant())
            {
                case "1":
                    this.SpawnAddAdditionalDataDialog();
                    break;
                case "2":
                    Console.WriteLine("You wish to use the existing data.");
                    Console.WriteLine("Unfortunately this option is not yet implemented at this time!");
                    break;
                case "e":
                case "exit":
                    return false; //user wishes to exit
            }

            return true;
        }

        private void SpawnAddAdditionalDataDialog()
        {
            var dialogModel = new AddAdditionalDataModel(this.Model.Data);
            var dialogController = new AddAdditionalDataController(dialogModel);
            var dialogView = new AddAdditionalDataView(dialogModel);
            dialogView.GetPathToPstFiles();
        }

        public static bool IsUserTryingToExit(string userInput)
        {
            switch(userInput.ToLowerInvariant())
            {
                case "e":
                case "exit":
                    return true;
            }

            return false;
        }
    }
}

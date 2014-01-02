using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OutlookParserConsoleApp.Models;

namespace OutlookParserConsoleApp.Controllers
{
    public class MainMenuController : IController<MainMenuModel>
    {
        private readonly MainMenuModel mdl;

        public MainMenuController(MainMenuModel menuModel)
        {
            this.mdl = menuModel;
        }

        public MainMenuModel Model
        {
            get
            {
                return this.mdl;
            }
        }

        public void DeleteAllEmails()
        {
            this.Model.DeleteAllEmails();
        }
    }
}

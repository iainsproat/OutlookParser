using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OutlookParserConsoleApp.Models;

namespace OutlookParserConsoleApp.Controllers
{
    public class AddAdditionalDataController : IController<AddAdditionalDataModel>
    {
        private readonly AddAdditionalDataModel mdl;

        public AddAdditionalDataController(AddAdditionalDataModel model)
        {
            this.mdl = model;
        }

        public AddAdditionalDataModel Model
        {
            get
            {
                return this.mdl;
            }
        }

        public void PathToPstFilesEntered(string userPath)
        {
            this.Model.ParsePathToPstFiles(userPath);
        }
    }
}

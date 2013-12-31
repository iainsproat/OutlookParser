using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OutlookParserConsoleApp.Models;

namespace OutlookParserConsoleApp.Controllers
{
    public interface IController
    {
        // empty
    }

    public interface IController<TModel> : IController
        where TModel : IModel
    {
        TModel Model { get; }
    }
}

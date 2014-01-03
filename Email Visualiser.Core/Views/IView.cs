using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmailVisualiser.Controllers;
using EmailVisualiser.Models;

namespace OutlookParserConsoleApp.Views
{
    public interface IView
    {
        // empty
    }

    public interface IView<TController, TModel> : IView
        where TController : IController
        where TModel : IModel
    {
        TController Controller { get; set; }
        TModel Model { get; }
        void Register(TModel model);
        void Release(TModel model);
    }
}

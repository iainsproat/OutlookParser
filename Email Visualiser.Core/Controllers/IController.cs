using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmailVisualiser.Models;

namespace EmailVisualiser.Controllers
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

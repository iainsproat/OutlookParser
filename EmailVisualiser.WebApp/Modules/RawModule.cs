using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmailVisualiser.WebApp.Models;
using EmailVisualiser.Data;
using Nancy;

namespace EmailVisualiser.WebApp.Modules
{
    public class RawModule : NancyModule
    {
        public RawModule(DataStorage data)
            :base("/raw")
        {
            Get["/"] = parameters =>
                {
                    return View["raw", new RawModel(data)];
                };
        }
    }
}

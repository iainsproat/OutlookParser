using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmailVisualiser.Data;
using EmailVisualiser.WebApp.Models;
using Nancy;

namespace EmailVisualiser.WebApp
{
    public class PulseModule : NancyModule
    {
        public PulseModule()
        {
            Get["/"] = parameters =>
                {
                    DataStorage data = new DataStorage();
                    return View["pulse", new PulseModel(data)];
                };
            Get["/js/{File}"] = parameters =>
                {
                    return Response.AsJs("Scripts/" + parameters.File as string);
                };
            Get["/css/{File}"] = parameters =>
                {
                    return Response.AsCss("Content/" + parameters.File as string);
                };            
        }
    }
}

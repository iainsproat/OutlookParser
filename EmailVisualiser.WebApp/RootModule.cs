using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nancy;

namespace EmailVisualiser.WebApp
{
    public class RootModule : NancyModule
    {
        public RootModule()
        {
            Get["/"] = parameters =>
                {
                    return View["index.cshtml"];
                };           
            Get["/js/{File}"] = parameters =>
                {
                    return Response.AsJs("Scripts/" + parameters.File as string);
                };
            Get["/css/{File}"] = parameters =>
                {
                    return Response.AsCss("Content/" + parameters.File as string);
                };
            Get["/data/{File}"] = parameters =>
                {
                    return Response.AsFile("Data/" + parameters.File as string + ".json");
                };
        }
    }
}

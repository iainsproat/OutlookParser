using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmailVisualiser.Data;
using Nancy;

namespace EmailVisualiser.WebApp
{
    public class DeleteModule : NancyModule
    {
        public DeleteModule(DataStorage data)
            :base("/delete")
        {
            Get["/all"] = parameters =>
                {
                    return View["delete.cshtml"];
                };

            Post["/all"] = parameters =>
                {
                    data.DeleteAll();
                    return Response.AsRedirect("~/", type: Nancy.Responses.RedirectResponse.RedirectType.SeeOther);
                };
        }
    }
}

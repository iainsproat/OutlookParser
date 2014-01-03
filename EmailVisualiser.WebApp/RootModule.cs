using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nancy;

using EmailVisualiser.Analysis;
using EmailVisualiser.Data;
using EmailVisualiser.Models;

namespace EmailVisualiser.WebApp
{
    public class RootModule : NancyModule
    {
        public RootModule(DataAnalysisEngine analysisEngine)
        {
            Get["/"] = parameters =>
                {
                    DataStorage data = new DataStorage();
                    return View["index.cshtml", new MainMenuModel(data)];
                };
            Get["/js/{File}"] = parameters =>
                {
                    return Response.AsJs("Scripts/" + parameters.File as string);
                };
            Get["/css/{File}"] = parameters =>
                {
                    return Response.AsCss("Content/" + parameters.File as string);
                };
            Get["/data/{File}"] = parameters => //TODO no need to refer to a file anymore!
                {
                    var dailyEmails = analysisEngine.GetEmailDailyCountSortedByDate();
                    return Response.AsJson(dailyEmails.Select(dailyEmail =>
                        {
                            return new
                            {
                                x = dailyEmail.Item1.ToUniversalTime().Subtract(
                                    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                                    ).TotalMilliseconds / 1000, //seconds
                                y = dailyEmail.Item2
                            };
                        }));
                };
        }
    }
}

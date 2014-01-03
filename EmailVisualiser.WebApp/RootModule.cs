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
                    var dailyEmails = analysisEngine.GetEmailDailyCountSortedByDate();
                    return Response.AsJson(dailyEmails.Select(dailyEmail =>
                        {
                            return new { x = dailyEmail.Item1.DayOfYear, y = dailyEmail.Item2 };
                        }));
                };
        }
    }
}

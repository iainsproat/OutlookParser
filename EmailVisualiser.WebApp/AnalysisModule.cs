using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmailVisualiser.Models;
using Nancy;

namespace EmailVisualiser.WebApp
{
    public class AnalysisModule : NancyModule
    {
        public AnalysisModule(DataVisualisationModel model)
            : base("/analysis")
        {
            Get["/"] = parameters =>
            {
                return View["analysis"];
            };

            Get["/data"] = parameters => //TODO no need to refer to a file anymore!
            {
                var dailyEmails = model.GetEmailDailyCountSortedByDate();
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

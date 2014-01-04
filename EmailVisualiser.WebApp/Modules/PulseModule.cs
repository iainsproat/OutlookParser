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
        private readonly DataStorage _data;
        private readonly PulseModel _model;
        public PulseModule()
        {
            this._data = new DataStorage();
            this._model = new PulseModel(this._data);

            Get["/"] = parameters =>
                {
                    return View["pulse", this._model];
                };
            Get["/js/{File}"] = parameters =>
                {
                    return Response.AsJs("Scripts/" + parameters.File as string);
                };
            Get["/css/{File}"] = parameters =>
                {
                    return Response.AsCss("Content/" + parameters.File as string);
                };
            Get["/data/SentEmailCount_per_user"] = parameters =>
                {
                    var outgoingCounts = this._model.OutgoingCountPerSender();
                    int i = 0;
                    return Response.AsJson(outgoingCounts.Select(uc =>
                    {                        
                        var temp = new
                        {
                            x = i,
                            xAxisName = uc.Item1,
                            y = uc.Item2
                        };
                        i++;
                        return temp;
                    }));
                };
        }
    }
}

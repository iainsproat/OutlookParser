using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmailVisualiser.Models;
using EmailVisualiser.Analysis.Graph;
using Nancy;

namespace EmailVisualiser.WebApp
{
    public class AnalysisModule : NancyModule
    {
        public AnalysisModule(DataVisualisationModel model)
            : base("/graphs")
        {
            Get["/"] = parameters =>
            {
                return View["graphs"];
            };

            Get["/data/EmailDailyCountSortedByDate"] = parameters =>
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

            Get["/data/GraphOfSendersAndRecipients"] = parameters =>
                {
                    var graph = model.WeightedGraphOfSendersAndRecipients() as WeightedGraph<string>;
                    return Response.AsJson(new D3JsGraph(graph));
                };
        }

        class D3JsGraph
        {
            private readonly List<Node> _nodes = new List<Node>();
            private readonly List<Link> _links = new List<Link>();

            public D3JsGraph(WeightedGraph<string> graph)
            {
                foreach(var vertex in graph.Vertices)
                {
                    this._nodes.Add(new Node(vertex));
                }

                foreach(var edge in graph.WeightedEdges)
                {
                    var link = new Link()
                    {
                       source = _nodes.FindIndex(node => {
                           return node.name == edge.Start;
                       }),
                       target = _nodes.FindIndex(node => {
                           return node.name == edge.End;
                       }),
                       value = edge.Weight
                    };
                    this._links.Add(link);
                }
            }

            public IEnumerable<Node> nodes
            { 
                get 
                { 
                    return this._nodes; 
                }
            }

            public IEnumerable<Link> links 
            { 
                get
                {
                    return this._links;
                }
            }

            public class Node
            {
                public Node(string vertex)
                {
                    this.name = vertex;
                    this.group = Data.DataStorage.IsInternalEmailAddress(vertex) ? 0 : 1;
                }

                public string name { get; private set; }
                public int group { get; private set; }
            }

            public class Link
            {
                public int source { get; set; }
                public int target { get; set; }
                public int value { get; set; }
            }
        }

        
    }
}

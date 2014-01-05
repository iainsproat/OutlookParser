using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailVisualiser.Analysis.Graph
{
    public class WeightedGraph<TVertex> : GraphBase<TVertex>
    {
        protected readonly IDictionary<IEdge<TVertex>, int> _edgeWeights = new Dictionary<IEdge<TVertex>, int>();

        public override IEnumerable<IEdge<TVertex>> Edges
        {
            get
            {
                return this._edgeWeights.Keys;
            }
        }

        public IEnumerable<WeightedEdge<TVertex>> WeightedEdges
        {
            get
            {
                return this._edgeWeights.Select(kvp =>
                {
                    return new WeightedEdge<TVertex>(kvp.Key.Start, kvp.Key.End, kvp.Value);
                });
            }
        }

        public class WeightedEdge<TVertex> : UndirectedEdge<TVertex>
        {
            public WeightedEdge(TVertex start, TVertex end, int weightValue)
                : base(start, end)
            {
                this.Weight = weightValue;
            }

            public int Weight { get; private set; }
        }

        public override void Connect(TVertex start, TVertex end)
        {
            this.AddEdge(new UndirectedEdge<TVertex>(start, end));
        }

        /// <summary>
        /// The weight of an edge is increased by 1 if the edge is already contained in the graph.
        /// </summary>
        /// <param name="edgeToAdd"></param>
        public override void AddEdge(IEdge<TVertex> edgeToAdd)
        {
            if (edgeToAdd == null)
            {
                throw new ArgumentNullException("edgeToAdd");
            }

            if (edgeToAdd.Start == null)
            {
                throw new ArgumentException("edgeToAdd.Start cannot be null");
            }

            if (edgeToAdd.End == null)
            {
                throw new ArgumentException("edgeToAdd.End cannot be null");
            }

            if (this._edgeWeights.ContainsKey(edgeToAdd))
            {
                this._edgeWeights[edgeToAdd]++; //already contains the edge, so increment the weight
                return;
            }

            var undirectedEdge = edgeToAdd as UndirectedEdge<TVertex>;
            if (undirectedEdge == null)
            {
                return;
            }

            if (!this._vertices.Contains(edgeToAdd.Start))
            {
                this._vertices.Add(edgeToAdd.Start);
            }

            if (!this._vertices.Contains(edgeToAdd.End))
            {
                this._vertices.Add(edgeToAdd.End);
            }

            this._edgeWeights.Add(edgeToAdd, 1);
        }
    }
}

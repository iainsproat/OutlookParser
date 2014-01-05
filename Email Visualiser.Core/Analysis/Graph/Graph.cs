using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailVisualiser.Analysis.Graph
{
    public class Graph<TVertex> : GraphBase<TVertex>
    {
        //TODO add orphaned vertice method
        //TODO remove edge methods, with a flag to determine whether resulting orphaned vertices should be removed
        protected readonly IList<IEdge<TVertex>> _edges = new List<IEdge<TVertex>>();
        
        public override IEnumerable<IEdge<TVertex>> Edges
        {
            get
            {
                return this._edges;
            }
        }

        public override void Connect(TVertex start, TVertex end)
        {
            this.AddEdge(new Edge<TVertex>(start, end));
        }

        public override void AddEdge(IEdge<TVertex> edgeToAdd)
        {
            if(edgeToAdd == null)
            {
                throw new ArgumentNullException("edgeToAdd");
            }

            if(edgeToAdd.Start == null)
            {
                throw new ArgumentException("edgeToAdd.Start cannot be null");
            }

            if(edgeToAdd.End == null)
            {
                throw new ArgumentException("edgeToAdd.End cannot be null");
            }

            // this.Edges can contain duplicate edges

            var undirectedEdge = edgeToAdd as UndirectedEdge<TVertex>;
            if(undirectedEdge == null)

            if (!this._vertices.Contains(edgeToAdd.Start))
            {
                this._vertices.Add(edgeToAdd.Start);
            }

            if (!this._vertices.Contains(edgeToAdd.End))
            {
                this._vertices.Add(edgeToAdd.End);
            }

            this._edges.Add(edgeToAdd);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailVisualiser.Analysis.Graph
{
    public abstract class GraphBase<TVertex> : IGraph<TVertex>
    {
        protected readonly IList<TVertex> _vertices = new List<TVertex>();

        public IEnumerable<TVertex> Vertices
        {
            get
            {
                return this._vertices;
            }
        }

        public abstract IEnumerable<IEdge<TVertex>> Edges { get; }

        public abstract void Connect(TVertex start, TVertex end);

        public abstract void AddEdge(IEdge<TVertex> edgeToAdd);

        public IEnumerable<IEdge<TVertex>> AllConnections(TVertex vertex)
        {
            return this.Edges.Where(e =>
            {
                return e.End.Equals(vertex) || e.Start.Equals(vertex);
            });
        }

        public IEnumerable<IEdge<TVertex>> IncomingConnections(TVertex vertex)
        {
            return this.Edges.Where(e =>
            {
                return e.End.Equals(vertex);
            });
        }

        public IEnumerable<IEdge<TVertex>> OutgoingConnections(TVertex vertex)
        {
            return this.Edges.Where(e =>
            {
                return e.Start.Equals(vertex);
            });
        }
    }
}

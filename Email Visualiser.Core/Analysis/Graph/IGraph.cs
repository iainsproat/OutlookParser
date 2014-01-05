using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailVisualiser.Analysis.Graph
{
    public interface IGraph<TVertex>
    {
        IEnumerable<TVertex> Vertices { get; }
        IEnumerable<IEdge<TVertex>> Edges { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailVisualiser.Analysis.Graph
{
    public interface IEdge<TVertex>
    {
        TVertex Start { get; }
        TVertex End { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailVisualiser.Analysis.Graph
{
    public class UndirectedEdge<TVertex> : Edge<TVertex>, IEquatable<UndirectedEdge<TVertex>>
    {
        public UndirectedEdge(TVertex oneEnd, TVertex otherEnd)
            :base(oneEnd, otherEnd)
        {
            // empty
        }

        public override bool Equals(IEdge<TVertex> other)
        {
            if (other == null)
            {
                return false;
            }

            if (base.StartsAreEqual(other))
            {
                return base.EndsAreEqual(other); //same as base.Equals(other)
            }
            else if (this.Start.Equals(other.End))
            {
                return (Object.ReferenceEquals(this.End, null) && Object.ReferenceEquals(other.Start, null)) || this.End.Equals(other.Start); //the reverse direction
            }

            return false;
        }

        public bool Equals(UndirectedEdge<TVertex> other)
        {
            return this.Equals(other as IEdge<TVertex>);
        }

        public override int GetHashCode()
        {
            int hash = 034077;
            unchecked
            {
                if (!Object.ReferenceEquals(this.Start, null))
                {
                    hash += 458 * this.Start.GetHashCode();
                }
                else
                {
                    hash += 287;
                }

                if (!Object.ReferenceEquals(this.End, null))
                {
                    hash += 458 * this.End.GetHashCode();
                }
                else
                {
                    hash += 287;
                }
            }

            return hash;
        }

        public IEdge<TVertex> Reverse()
        {
            return new UndirectedEdge<TVertex>(this.End, this.Start);
        }
    }
}

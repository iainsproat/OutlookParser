using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailVisualiser.Analysis.Graph
{
    public class Edge<TVertex> : IEdge<TVertex>, IEquatable<IEdge<TVertex>>, IEquatable<Edge<TVertex>>
    {
        public Edge(TVertex from, TVertex to)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }

            if (to == null)
            {
                throw new ArgumentNullException("to");
            }

            this.Start = from;
            this.End = to;
        }

        public TVertex Start
        {
            get;
            private set;
        }

        public TVertex End
        {
            get;
            private set;
        }

        public override bool Equals(object obj)
        {
            var other = obj as IEdge<TVertex>;
            return this.Equals(other);
        }

        public virtual bool Equals(IEdge<TVertex> other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.StartsAreEqual(other))
            {
                return this.EndsAreEqual(other);
            }

            return false;
        }

        protected bool StartsAreEqual(IEdge<TVertex> other)
        {
            return (Object.ReferenceEquals(this.Start, null) && Object.ReferenceEquals(other.Start, null)) || this.Start.Equals(other.Start);
        }

        protected bool EndsAreEqual(IEdge<TVertex> other)
        {
            return (Object.ReferenceEquals(this.End, null) && Object.ReferenceEquals(other.End, null)) || this.End.Equals(other.End);
        }

        public bool Equals(Edge<TVertex> other)
        {
            return this.Equals(other as IEdge<TVertex>);
        }

        public override int GetHashCode()
        {
            int hash = 03402;
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
                    hash += 32689 * this.End.GetHashCode();
                }
                else
                {
                    hash += 39;
                }
            }

            return hash;
        }
    }
}

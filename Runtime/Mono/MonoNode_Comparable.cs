using System;
using AceLand.Library.Extensions;
using AceLand.NodeFramework.Core;

namespace AceLand.NodeFramework.Mono
{
    public abstract partial class MonoNode<T>
    {
        public bool Equals(INode other)
        {
            if (other == null) return false;
            if (other.Id.IsNullOrEmptyOrWhiteSpace()) return false;
            if (this.Id.IsNullOrEmptyOrWhiteSpace()) return false;
            
            return Id == other.Id && this.GetType() == other.GetType();
        }

        // Override Object.Equals
        public override bool Equals(object obj)
        {
            if (obj is INode other)
                return Equals(other);
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        // IComparable<MyClass>
        public int CompareTo(INode other)
        {
            if (other == null) return 1;
            if (other.Id.IsNullOrEmptyOrWhiteSpace()) return 1;
            if (this.Id.IsNullOrEmptyOrWhiteSpace()) return 1;
            if (this.GetType() != other.GetType()) return 1;
            
            return string.Compare(Id, other.Id); 
        }

        // IComparable
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is not INode other) return 1;
            if (other.Id.IsNullOrEmptyOrWhiteSpace()) return 1;
            if (other.Id.IsNullOrEmptyOrWhiteSpace()) return 1;
            if (this.GetType() != obj.GetType()) return 1;

            return CompareTo(other);
            throw new ArgumentException("Object is not a MyClass");
        }
    }
}

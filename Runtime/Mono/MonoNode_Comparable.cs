using System;
using AceLand.Library.Extensions;
using AceLand.NodeFramework.Core;

namespace AceLand.NodeFramework.Mono
{
    public abstract partial class MonoNode<T>
    {
        public new bool Equals(INode other)
        {
            if (other == null) return false;
            if (other.Id.IsNullOrEmptyOrWhiteSpace()) return false;
            if (Id.IsNullOrEmptyOrWhiteSpace()) return false;
            
            return Id == other.Id && GetType() == other.GetType();
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
        public new int CompareTo(INode other)
        {
            if (other == null) return 1;
            if (other.Id.IsNullOrEmptyOrWhiteSpace()) return 1;
            if (Id.IsNullOrEmptyOrWhiteSpace()) return 1;
            if (GetType() != other.GetType()) return 1;
            
            return string.CompareOrdinal(Id, other.Id); 
        }

        // IComparable
        public new int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is not INode other) return 1;
            if (other.Id.IsNullOrEmptyOrWhiteSpace()) return 1;
            if (other.Id.IsNullOrEmptyOrWhiteSpace()) return 1;
            if (GetType() != obj.GetType()) return 1;

            return CompareTo(other);
        }
    }
}

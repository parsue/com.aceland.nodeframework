using System;
using AceLand.NodeFramework.Core;

namespace AceLand.NodeFramework.Mono
{
    public abstract partial class MonoNode<T>
    {
        public bool Equals(INode other)
        {
            if (other == null) return false;
            return Id == other.Id;
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
            return string.Compare(Id, other.Id); 
        }

        // IComparable
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is INode other)
                return CompareTo(other);

            throw new ArgumentException("Object is not a MyClass");
        }
    }
}

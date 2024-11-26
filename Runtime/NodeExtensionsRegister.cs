using AceLand.NodeFramework.Core;

namespace AceLand.NodeFramework
{
    public static partial class NodeExtensions
    {
        public static void Register<T>(this INode<T> node) where T : class, INode =>
            Nodes.Register(node.Concrete);
        
        public static void Unregister<T>(this INode<T> node) where T : class, INode =>
            Nodes.Register(node.Concrete);

        public static bool IsRegistered<T>(this INode<T> node) where T : class, INode =>
            Nodes.Contains<T>(node.Concrete);
    }
}

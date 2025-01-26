using System.Linq;
using UnityEngine;

namespace AceLand.NodeFramework.Mono
{
    [AddComponentMenu("AceLand/Node/Container Node")]
    public class ContainerNode : MonoNode<ContainerNode>
    {
        // empty MonoNode
        // use on structure-use function-less GameObject
        // for filling up gaps in GameObject structure

        protected override void StartAfterNodeBuilt()
        {
            base.StartAfterNodeBuilt();

            if (this.IsRoot())
            {
                Debug.Log($"ContainerNode {Id} | Active: {Go.activeInHierarchy} | isRoot | {this.Children().Count()}");
                return;
            }
            
            Debug.Log($"ContainerNode {Id} | Active: {Go.activeInHierarchy} | {this.Parent().Id} | {this.Children().Count()}");
        }
    }
}
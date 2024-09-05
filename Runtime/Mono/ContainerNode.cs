using UnityEngine;

namespace AceLand.NodeSystem.Mono
{
    [AddComponentMenu("AMVR/Node/Container Node")]
    public class ContainerNode : NodeMono<ContainerNode>
    {
        public string NodeId => nodeId;
    }
}
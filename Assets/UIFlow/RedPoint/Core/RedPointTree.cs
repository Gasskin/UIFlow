using System.Collections.Generic;

namespace UIFlow.RedPoint
{
    public class RedPointNode
    {
        public RedPointNode parentNode;
        public Dictionary<int, RedPointNode> childNodes;
        public RedPointNodeConfig nodeConfig;

        public RedPointNode(RedPointNodeConfig nodeConfig)
        {
            this.nodeConfig = nodeConfig;
            childNodes = new Dictionary<int, RedPointNode>();
        }

        public void AddChild(RedPointNode childNode)
        {
            childNode.parentNode = this;
            childNodes.Add(childNode.nodeConfig.selfId, childNode);
        }
    }
    
    public class RedPointTree
    {
        public Dictionary<int, RedPointNode> nodes;
        private List<int> removeHelper;
        private bool hasParent = false;

        public RedPointTree()
        {
            nodes = new Dictionary<int, RedPointNode>();
            removeHelper = new List<int>();
        }

        public void AddNodeConfig(RedPointNodeConfig nodeConfig)
        {
            if (nodeConfig.parentId == 0)
            {
                AddRoot(nodeConfig);
            }
            else
            {
                AddChild(nodeConfig);
            }
        }

        /// <summary>
        /// 加入一个根节点，移动所有需要的子节点到它下面
        /// </summary>
        /// <param name="nodeConfig"></param>
        private void AddRoot(RedPointNodeConfig nodeConfig)
        {
            removeHelper.Clear();
            
            var node = new RedPointNode(nodeConfig);
            
            // 找到根目录里该节点所有的子节点
            foreach (var nodePair in nodes)
            {
                if (nodePair.Key == nodeConfig.selfId)
                {
                    removeHelper.Add(nodePair.Key);
                    node.AddChild(nodePair.Value);
                }
            }
            
            // 把节点添加到根目录
            nodes.Add(nodeConfig.selfId, node);


            // 从根目录移除
            foreach (var id in removeHelper)
            {
                nodes.Remove(id);
            }
        }

        /// <summary>
        /// 加入一个子节点，尝试找到他的父节点，如果没有，那么直接添加到根目录
        /// </summary>
        /// <param name="nodeConfig"></param>
        private void AddChild(RedPointNodeConfig nodeConfig)
        {
            var node = new RedPointNode(nodeConfig);
            hasParent = false;
            TryFindParent(nodes,node);
            if (!hasParent)
            {
                nodes.Add(nodeConfig.selfId, node);
            }
        }


        private void TryFindParent(Dictionary<int, RedPointNode> nodes, RedPointNode node)
        {
            foreach (var nodePair in nodes)
            {
                if (hasParent)
                    return;
                if (nodePair.Key == node.nodeConfig.parentId)
                {
                    nodePair.Value.AddChild(node);
                    hasParent = true;
                    return;
                }
                TryFindParent(nodePair.Value.childNodes,node);
            }
        }
    }
}
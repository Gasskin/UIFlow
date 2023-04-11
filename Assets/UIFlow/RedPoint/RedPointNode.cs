using System;
using UnityEngine.Serialization;

namespace UIFlow.RedPoint
{
    [Serializable]

    public class RedPointNode
    {
#if UNITY_EDITOR
        public string name;
        public bool confirm;
#endif
        public int selfId;
        public int parentId;
    }
}

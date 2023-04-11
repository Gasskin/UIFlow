using System;
using UnityEngine.Serialization;

namespace UIFlow.RedPoint
{
    public enum RedPointType
    {
        Point = 1,
        Num = 2,
    }
    
    [Serializable]

    public class RedPointNodeConfig
    {
#if UNITY_EDITOR
        public string name;
        public bool confirm;
#endif
        public RedPointType redPointType = RedPointType.Point;
        public int offsetX;
        public int offsetY;
        public int selfId;
        public int parentId;
    }
}

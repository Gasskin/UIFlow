using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UIFlow.RedPoint
{
    public class RedPoint
    {
        public int num;
        public RedPointNodeConfig config;

        public RedPoint(RedPointNodeConfig config)
        {
            this.config = config;
            num = 0;
        }
    }

    public class RedPointManager : Singleton<RedPointManager>
    {
    #region 属性
        private RedPointConfig config;
        private RedPointTree redPointTree;
        private Dictionary<int, RedPoint> redPointMap;
        private Transform pool;
        private Queue<Transform> pointQueue;
        private Queue<Transform> numQueue;
        private Dictionary<int, Transform> redPointWatcher;
        private Dictionary<int, Transform> redPointInstance;
        private GameObject numAsset;
        private GameObject pointAsset;
        private bool isInit = false;
    #endregion

    #region 生命周期
        public async void Init()
        {
            if (isInit)
                return;
            config = await Resources.LoadAsync<RedPointConfig>("RedPointConfig") as RedPointConfig;
            if (config == null)
            {
                Debug.LogError("加载RedPointConfig失败");
                return;
            }

            redPointMap = new Dictionary<int, RedPoint>();
            foreach (var nodeConfig in config.nodes)
            {
                redPointMap.Add(nodeConfig.selfId, new RedPoint(nodeConfig));
            }

            var go = new GameObject("Pool");
            go.SetActive(false);
            pool = go.transform;
            pool.SetParent(transform, false);

            pointQueue = new Queue<Transform>();
            numQueue = new Queue<Transform>();
            redPointWatcher = new Dictionary<int, Transform>();
            redPointInstance = new Dictionary<int, Transform>();

            isInit = true;
        }
    #endregion

    #region 接口方法
        /// <summary>
        /// 刷新红点树
        /// </summary>
        /// <param name="selfId">目标节点的ID</param>
        /// <param name="num">增删数量</param>
        public void RefreshRedPoint(int selfId, int num)
        {
            if (!isInit)
                return;
            if (redPointMap.TryGetValue(selfId, out var redPoint))
            {
                redPoint.num += num;
                AddOrFreshRedPoint(selfId, redPoint);
                if (redPoint.config.parentId != 0)
                {
                    RefreshRedPoint(redPoint.config.parentId, num);
                }
            }
        }

        /// <summary>
        /// 添加红点观察
        /// </summary>
        /// <param name="selfId">节点ID</param>
        /// <param name="target">挂载目标</param>
        public void AddRedPointWatcher(int selfId, Transform target)
        {
            if (redPointWatcher.ContainsKey(selfId))
            {
                Debug.LogError($"一个红点只能添加一次观察  {selfId}  {target.name}");
                return;
            }

            redPointWatcher.Add(selfId, target);
        }

        /// <summary>
        /// 移除红点观察
        /// </summary>
        /// <param name="selfId">节点ID</param>
        public void RemoveRedPointWatcher(int selfId)
        {
            if (redPointWatcher.Remove(selfId))
            {
                if (redPointMap.TryGetValue(selfId, out var redPoint))
                {
                    if (redPointInstance.TryGetValue(selfId, out var instance))
                    {
                        ReCycleRedPoint(redPoint.config.redPointType, instance);
                    }
                }
            }
        }
    #endregion

    #region 工具方法
        private void AddOrFreshRedPoint(int selfId, RedPoint redPoint)
        {
            if (redPointInstance.TryGetValue(selfId,out var instance))
            {
                switch (redPoint.config.redPointType)
                {
                    case RedPointType.Num:
                        break;
                    case RedPointType.Point:
                        break;
                }
            }
            else
            {
                switch (redPoint.config.redPointType)
                {
                    case RedPointType.Num:
                        break;
                    case RedPointType.Point:
                        break;
                }
            }
        }

        private void ReCycleRedPoint(RedPointType redPointType, Transform instance)
        {
            instance.SetParent(pool, false);
            switch (redPointType)
            {
                case RedPointType.Num:
                    numQueue.Enqueue(instance);
                    break;
                case RedPointType.Point:
                    pointQueue.Enqueue(instance);
                    break;
            }
        }
    #endregion
    }
}
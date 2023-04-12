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
        private Transform pool;

        private Queue<Transform> pointQueue;
        private Queue<Transform> numQueue;
        private Dictionary<int, RedPoint> redPointMap;
        private Dictionary<int, Transform> redPointWatcher;
        private Dictionary<int, RedPointView> redPointView;

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
            redPointView = new Dictionary<int, RedPointView>();

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
                redPoint.num = Mathf.Max(0, redPoint.num);
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
        public void AddWatcher(int selfId, Transform target)
        {
            if (!isInit)
                return;
            
            if (redPointWatcher.ContainsKey(selfId))
            {
                Debug.LogError($"一个红点只能添加一次观察  {selfId}  {target.name}");
                return;
            }

            if (!redPointMap.TryGetValue(selfId, out var redPoint))
            {
                Debug.LogError($"不存在红点  {selfId}");
                return;
            }

            redPointWatcher.Add(selfId, target);

            GetOrCreateRedPoint(selfId, redPoint, target);
        }

        /// <summary>
        /// 移除红点观察
        /// </summary>
        /// <param name="selfId">节点ID</param>
        public void RemoveWatcher(int selfId)
        {
            if (!isInit)
                return;
            
            if (redPointWatcher.Remove(selfId))
            {
                if (redPointMap.TryGetValue(selfId, out var redPoint))
                {
                    if (redPointView.TryGetValue(selfId, out var view))
                    {
                        redPointView.Remove(selfId);
                        ReCycleRedPoint(redPoint.config.redPointType, view.transform);
                    }
                }
            }
        }

    #endregion

    #region 工具方法

        private void AddOrFreshRedPoint(int selfId, RedPoint redPoint)
        {
            // 已经创建过红点了
            if (redPointView.TryGetValue(selfId, out var view))
            {
                view.Refresh(redPoint.num);
            }
            else
            {
                if (redPointWatcher.TryGetValue(selfId, out var target))
                {
                    GetOrCreateRedPoint(selfId, redPoint, target);
                }
            }
        }

        private void GetOrCreateRedPoint(int selfId, RedPoint redPoint, Transform target)
        {
            Transform instance = null;

            switch (redPoint.config.redPointType)
            {
                case RedPointType.Num:
                    instance = numQueue.Count > 0 ? numQueue.Dequeue() : Instantiate(config.numPrefab).transform;
                    break;
                case RedPointType.Point:
                    instance = pointQueue.Count > 0 ? pointQueue.Dequeue() : Instantiate(config.pointPrefab).transform;
                    break;
            }

            if (instance && instance.TryGetComponent<RedPointView>(out var view))
            {
                instance.SetParent(target, false);
                redPointView.Add(selfId, view);
                view.Refresh(redPoint.num);

                if (instance is RectTransform rect)
                {
                    rect.anchorMin = Vector2.one;
                    rect.anchorMax = Vector2.one;
                    rect.anchoredPosition = new Vector2(redPoint.config.offsetX, redPoint.config.offsetY);
                }
            }
            else
            {
                DestroyImmediate(instance);
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
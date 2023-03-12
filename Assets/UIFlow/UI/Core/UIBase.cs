using System.Collections.Generic;
using UnityEngine;

namespace UIFlow.UI
{
    public abstract class UIBase
    {
    #region 字段
        protected GameObject prefabInstance;
        protected RectTransform rectTransform;
        protected CanvasGroup canvasGroup;
        protected List<UIBase> subUI = new List<UIBase>();
        protected object uiParam;
    #endregion

    #region 属性
        public bool IsOpen => canvasGroup.alpha >= 1;
        public abstract UIType Layer { get; }
        public UIBase preUI { get; private set; }

        protected abstract string PrefabName { get; }
    #endregion

    #region 生命周期
        public virtual bool BindComponent()
        {
            return true;
        }

        public void Load(GameObject prefabInstance)
        {
            this.prefabInstance = prefabInstance;
            rectTransform = this.prefabInstance.transform as RectTransform;
            if (rectTransform != null)
            {
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.anchoredPosition = Vector2.one;
            }

            if (!this.prefabInstance.TryGetComponent(out canvasGroup))
            {
                canvasGroup = this.prefabInstance.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0;
            OnLoad();
        }

        public void Show(UIBase preUI, object uiParam)
        {
            this.preUI = preUI;
            canvasGroup.alpha = 1;
            if (uiParam != null)
                this.uiParam = uiParam;
            OnShow();
            OpenAllSubUI();
        }

        public void Close()
        {
            canvasGroup.alpha = 0;
            OnClose();
            CloseAllSubUI();
        }

        public void Unload()
        {
            Object.Destroy(prefabInstance);
            prefabInstance = null;
            canvasGroup = null;
            uiParam = null;
            subUI.Clear();
            OnUnload();
        }

        public void Update()
        {
            OnUpdate();
        }

        protected virtual void OnLoad()
        {
        }

        protected virtual void OnShow()
        {
        }

        protected virtual void OnClose()
        {
        }

        protected virtual void OnUnload()
        {
        }

        protected virtual void OnUpdate()
        {
        }
    #endregion

    #region 接口方法
        protected void AddSubUI<T>(Transform uiRoot) where T : UIBase, new()
        {
        }

        protected void RemoveSubUI<T>(Transform uiRoot) where T : UIBase, new()
        {
            
        }

        private void OpenAllSubUI()
        {
            
        }

        private void CloseAllSubUI()
        {
            
        }
    #endregion
    }
}
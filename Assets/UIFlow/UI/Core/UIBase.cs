using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFlow.UI
{
    public abstract class UIBase
    {
        protected abstract UIType uiType { get; }
        protected abstract string prefabName { get; }
        
        private List<UIBase> subUI = new List<UIBase>();

        public virtual void OnLoad()
        {
            
        }


        public virtual void OnShow()
        {
            
        }

        public virtual void OnClose()
        {
            
        }

        public virtual void OnUnload()
        {
            
        }
    }
}
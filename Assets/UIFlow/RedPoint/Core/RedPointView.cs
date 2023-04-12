using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFlow.RedPoint
{
    public class RedPointView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]       
        private Text redPointNum;

        private List<string> numStrList = new List<string>(99);

        private void OnEnable()
        {
            for (int i = 0; i < 99; i++)
            {
                numStrList.Add((i + 1).ToString());
            }
        }

        public void Refresh(int num)
        {
            if (num <= 0)
            {
                canvasGroup.alpha = 0;
                return;
            }

            canvasGroup.alpha = 1;
            if (redPointNum != null)
            {
                num = Math.Min(num, 99) - 1;
                redPointNum.text = numStrList[num];
            }
        }
    }
}
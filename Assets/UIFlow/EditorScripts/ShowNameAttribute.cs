using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFlow.Editor
{
    public class ShowNameAttribute : PropertyAttribute
    {
        public string displayName;

        public ShowNameAttribute(string displayName)
        {
            this.displayName = displayName;
        }
    }
}
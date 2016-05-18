﻿using UnityEngine;
using System.Collections;

namespace ResetCore.UGUI
{
    public class BasePopupUI : BaseUI
    {
        protected override void OnEnable()
        {
            uiRoot = UIManager.Instance.popupRoot;
            transform.SetParent(uiRoot, false);
            transform.SetAsLastSibling();
        }

    }

}


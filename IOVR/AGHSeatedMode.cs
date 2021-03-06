﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VRGIN.Controls;
using VRGIN.Core;
using VRGIN.Helpers;
using VRGIN.Modes;

namespace IOVR
{
    class AGHSeatedMode : SeatedMode
    {

        protected override IEnumerable<IShortcut> CreateShortcuts()
        {
            return base.CreateShortcuts().Concat(new IShortcut[] {
                new MultiKeyboardShortcut(new KeyStroke("Ctrl + C"), new KeyStroke("Ctrl + C"), ChangeModeOnControllersDetected ),
                new MultiKeyboardShortcut(new KeyStroke("Ctrl + C"), new KeyStroke("Ctrl + M"), delegate { GameObject.FindObjectsOfType<MozaicSetUp>().ToList().ForEach(m => m.MozaObj.enabled = false); } ),
            });
        }

        protected override void ChangeModeOnControllersDetected()
        {
            VR.Manager.SetMode<AGHStandingMode>();
        }

        protected override void InitializeTools(Controller controller, bool isLeft)
        {
            base.InitializeTools(controller, isLeft);

            //controller.gameObject.AddComponent<BubbleSelectionHandler>();
        }
    }
}

﻿using IllusionPlugin;
using IOVR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using VRGIN.Core;
using VRGIN.Helpers;
using VRGIN.Modes;

namespace IOVR
{
    public class IOVR : IEnhancedPlugin
    {
        public string[] Filter
        {
            get
            {
                return new string[] { "io" };
            }
        }

        public string Name
        {
            get
            {
                return "IOVR";
            }
        }

        public string Version
        {
            get
            {
                return "0.1";
            }
        }

        public void OnApplicationQuit()
        {
        }
        
        public void OnApplicationStart()
        {
            if (Environment.CommandLine.Contains("--vr"))
            {
                var context = new AGHContext();
                VRManager.Create<AGHInterpreter>(context);

                if ((VR.Settings as AGHSettings).UseOneHandedMode)
                {
                    VR.Manager.SetMode<OneHandedMode>();
                } else
                {
                    VR.Manager.SetMode<AGHSeatedMode>();
                }
            }
            VRLog.Info("Layers: " + string.Join(", ", UnityHelper.GetLayerNames(int.MaxValue)));
            //UnityEngine.SceneManagement.SceneManager.LoadScene(7);
        }

        public void OnFixedUpdate()
        {
        }

        public void OnLateUpdate()
        {
        }

        public void OnLevelWasInitialized(int level)
        {
            //VRLog.Info(string.Join(", ", GameObject.FindObjectsOfType<Renderer>().Select(r => r.name + ": "+ string.Join(" - ", r.sharedMaterials.Select(m => m.name).ToArray()) ).Distinct().ToArray()));
        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnUpdate()
        {
        }
    }
}

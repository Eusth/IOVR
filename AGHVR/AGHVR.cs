using IllusionPlugin;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VRGIN.Core;
using VRGIN.Modes;

namespace AGHVR
{
    public class AGHVR : IEnhancedPlugin
    {
        public string[] Filter
        {
            get
            {
                return new string[] { "AGH" };
            }
        }

        public string Name
        {
            get
            {
                return "AGHVR";
            }
        }

        public string Version
        {
            get
            {
                return "0.0";
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
                VR.Manager.SetMode<SeatedMode>();

            }
        }

        public void OnFixedUpdate()
        {
        }

        public void OnLateUpdate()
        {
        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnUpdate()
        {
   
        }
    }
}

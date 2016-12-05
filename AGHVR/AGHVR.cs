using IllusionPlugin;
using System;
using System.Collections.Generic;
using System.Text;
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
                VRLog.Info("OK1");
                var context = new AGHContext();
                VRLog.Info("OK2");

                VRManager.Create<AGHInterpreter>(context);
                VRLog.Info("OK3");

                VR.Manager.SetMode<SeatedMode>();
                VRLog.Info("OK4");

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

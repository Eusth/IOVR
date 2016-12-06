using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VRGIN.Controls.Speech;
using VRGIN.Core;
using VRGIN.Visuals;

namespace AGHVR
{
    class AGHContext : IVRManagerContext
    {
        DefaultMaterialPalette _Materials;
        VRSettings _Settings;
        public AGHContext()
        {
            _Materials = new DefaultMaterialPalette();
            bool isNew = !System.IO.File.Exists("vr_settings.xml");
            _Settings = VRSettings.Load<VRSettings>("vr_settings.xml");

            if (isNew)
            {
                _Settings.IPDScale = 5f;
                _Settings.Save();
            }
        }

        public bool GUIAlternativeSortingMode
        {
            get
            {
                return false;
            }
        }

        public float GuiFarClipPlane
        {
            get
            {
                return 10000f;
            }
        }

        public string GuiLayer
        {
            get
            {
                return "Default";
            }
        }

        public float GuiNearClipPlane
        {
            get
            {
                return 0.1f;
            }
        }

        public int IgnoreMask
        {
            get
            {
                return LayerMask.GetMask("NGUI_UI", "NGUI_CU", "Danmen");
            }
        }

        public string InvisibleLayer
        {
            get
            {
                return "Ignore Raycast";
            }
        }

        public IMaterialPalette Materials
        {
            get
            {
                return _Materials;
            }
        }

        public global::UnityEngine.Color PrimaryColor
        {
            get
            {
                return Color.cyan;
            }
        }

        public VRSettings Settings
        {
            get
            {
                return _Settings;
            }
        }

        public bool SimulateCursor
        {
            get
            {
                return true;
            }
        }

        public string UILayer
        {
            get
            {
                return "UI";
            }
        }

        public int UILayerMask
        {
            get
            {
                return LayerMask.GetMask(UILayer);
            }
        }

        public Type VoiceCommandType
        {
            get
            {
                return typeof(VoiceCommand);
            }
        }
    }
}

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
            _Settings = new VRSettings();// VRSettings.Load<VRSettings>("vr_settings.xml");
            _Settings.IPDScale = 5f;
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

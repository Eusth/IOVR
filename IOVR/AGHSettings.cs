using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using VRGIN.Core;

namespace IOVR
{
    [XmlRoot("Settings")]
    public class AGHSettings : VRSettings
    {
        public bool LookAtMe { get { return _LookAtMe; } set { _LookAtMe = value; } }
        private bool _LookAtMe = false;

        public bool UseOneHandedMode { get { return _UseOneHandedMode; } set { _UseOneHandedMode = value; } }
        private bool _UseOneHandedMode = false;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using VRGIN.Core;

namespace AGHVR
{
    [XmlRoot("Settings")]
    public class AGHSettings : VRSettings
    {
        public bool LookAtMe { get { return _LookAtMe; } set { _LookAtMe = value; } }
        private bool _LookAtMe = false;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Valve.VR;
using VRGIN.Controls;
using VRGIN.Core;

namespace AGHVR
{
    public class BubbleSelectionHandler : ProtectedBehaviour
    {
        Controller _Controller;
        private Controller.Lock _Lock;

        protected override void OnStart()
        {
            base.OnStart();

            _Controller = GetComponent<Controller>();
        }

        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            var colliders = Physics.OverlapSphere(transform.position, 0.2f * VR.Settings.IPDScale, LayerMask.GetMask("FreeUI"));

            if (colliders.Length > 0)
            {
                if (!HasLock() && _Controller.TryAcquireFocus(out _Lock))
                {
                    UICamera.currentScheme = UICamera.ControlScheme.Controller;
                    UICamera.selectedObject = colliders.First().gameObject;
                }
            }
            else if (HasLock())
            {
                UICamera.currentScheme = UICamera.ControlScheme.Mouse;
                UICamera.selectedObject = null;
                _Lock.Release();
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if(HasLock())
            {
                var device = SteamVR_Controller.Input((int)_Controller.Tracking.index);
                if(device.GetHairTriggerUp())
                {
                    UICamera.Notify(UICamera.selectedObject, "OnClick", null);
                }
            }
        }

        bool HasLock()
        {
            return _Lock != null && _Lock.IsValid;
        }
    }
}

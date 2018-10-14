using System;
using static SteamVR_Controller;

namespace IOVR
{
    public class InputScheme
    {
        public bool CheckGrabStart()
        {
            return Device.GetPressDown(ButtonMask.Grip);
        }



        public SteamVR_Controller.Device Device { get; internal set; }

        internal bool CheckPresetModeStart()
        {
            return Device.GetPressDown(ButtonMask.Trigger);
        }

        internal bool CheckPresetModeEnd()
        {
            return Device.GetPressUp(ButtonMask.Trigger);
        }

        internal bool IsUsingDirectionalInput()
        {
            return Device.GetTouch(ButtonMask.Touchpad);
        }
    }
}
﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Valve.VR;
using VRGIN.Controls;
using VRGIN.Core;
using VRGIN.Helpers;
using static SteamVR_Controller;

namespace IOVR
{
    internal class OneHandedController : Controller
    {
        private enum MainMode
        {
            Game,
            Teleport
        }

        private enum SubMode
        {
            Idle,
            Moving,
            Rotating
        }

        private SteamVR_TrackedObject.EIndex _index = SteamVR_TrackedObject.EIndex.None;
        private SteamVR_Controller.Device _input;
        private OneHandedMode _mode;

        private SubMode _subMode = SubMode.Idle;
        private MainMode _mainMode = MainMode.Game;

        public static OneHandedController Create(OneHandedMode mode)
        {
            var hand = new GameObject("Controller").AddComponent<OneHandedController>();
            hand._mode = mode;
            return hand;
        }

        protected override void OnUpdate()
        {
            if(_index != Tracking.index)
            {
                // Update device
                _index = Tracking.index;
                _input = SteamVR_Controller.Input((int)_index);
            }

            ProcessLock();

            if(CanAcquireFocus())
            {
                UpdateLogic();
            }
        }

        protected void UpdateLogic()
        {
            if (_input.GetPressDown(ButtonMask.Grip) && _subMode == SubMode.Idle)
            {
                StartCoroutine(GrabbingCoroutine());
            }

            if (_input.GetPressDown(ButtonMask.Trigger) && _subMode == SubMode.Idle)
            {
                _mode.ShowPresets();
                //StartCoroutine(TriggerCoroutine());
            } else if(_input.GetPressUp(ButtonMask.Trigger))
            {
                _mode.HidePresets();
            }

            if (_input.GetTouch(ButtonMask.Touchpad))
            {
                var touchpad = _input.GetAxis();
                if (touchpad.y > 0.8f && Mathf.Abs(touchpad.x) < 0.5f)
                {
                    _mode.ShowGUI(this);
                }
                else if (touchpad.y < -0.8f && Mathf.Abs(touchpad.x) < 0.5f)
                {
                    _mode.HideGUI();
                }
            }



         
        }
        
        //IEnumerator TriggerCoroutine()
        //{
        //    _mode.Stop();

        //    var origin = VR.Camera.Origin;
        //    var start = origin.rotation * transform.localPosition;
        //    var startPos = transform.position;

        //    KeyValuePair<Vector3, Vector3> startPositions = new KeyValuePair<Vector3, Vector3>();
        //    while (_input.GetPress(ButtonMask.Trigger))
        //    {
        //        if (_input.GetPressDown(ButtonMask.Grip))
        //        {
        //            startPositions = new KeyValuePair<Vector3, Vector3>(start, origin.rotation * transform.localPosition);
        //        } else if(Input.GetPress(ButtonMask.Grip))
        //        {
        //            start = startPositions.Key + (((origin.rotation * transform.localPosition) - startPositions.Value));
        //        }

        //        yield return null;
        //    }

        //    var end = origin.rotation * transform.localPosition;

        //    var v = (end - start).normalized;
        //    var rotation = Quaternion.LookRotation(v, Vector3.up);

        //    var startPosition = origin.position + start * VR.Settings.IPDScale;
        //    var camPos = VR.Camera.transform;

        //    origin.rotation = rotation * Quaternion.Inverse(camPos.localRotation);
        //    origin.position += (startPosition - camPos.position);
        //}

        IEnumerator GrabbingCoroutine()
        {
            var origin = VR.Camera.Origin;
            var oldOrigin = origin.position;
            var oldPosition = origin.rotation * transform.localPosition;
            _mode.Stop();


            yield return null;

            while (_input.GetPress(SteamVR_Controller.ButtonMask.Grip))
            {
                var newPosition = origin.rotation * transform.localPosition;
                var diff = (newPosition - oldPosition) * VR.Settings.IPDScale;
                var newOrigin = oldOrigin - diff;

                origin.position = newOrigin;

                yield return null;
            }

            if (_input.velocity.magnitude > 0.1f)
            {
                _mode.Velocity = -(origin.rotation * _input.velocity * VR.Settings.IPDScale);
            }

        }

        private List<CameraPreset> _hoveredElements = new List<CameraPreset>();

        void OnTriggerEnter(Collider other)
        {
            var preset = other.GetComponent<CameraPreset>();
            if (!preset) return;

            _hoveredElements.Add(preset);

            if (_mode.HoveredPreset != null) return;

            _mode.HoveredPreset = other.GetComponent<CameraPreset>();

            if (_mode.HoveredPreset)
            {
                _mode.HoveredPreset.Selected = true;
            }
        }


        void OnTriggerExit(Collider other)
        {
            var hoveredPreset = other.GetComponent<CameraPreset>();
            _hoveredElements.Remove(hoveredPreset);


            if (hoveredPreset == _mode.HoveredPreset)
            {
                _mode.HoveredPreset.Selected = false;
                _mode.HoveredPreset = null;
            }

            if(_hoveredElements.Count > 0)
            {
                _mode.HoveredPreset = _hoveredElements[0];
                _mode.HoveredPreset.Selected = true;
            }
        }

    }
}
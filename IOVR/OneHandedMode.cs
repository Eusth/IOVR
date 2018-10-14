using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Valve.VR;
using VRGIN.Controls;
using VRGIN.Core;
using VRGIN.Modes;
using VRGIN.Visuals;

namespace IOVR
{
    class OneHandedMode : StandingMode
    {
        public Vector3 Velocity = Vector3.zero;

        private GUIQuad _GUI;

        public CameraPreset HoveredPreset;
        public override IEnumerable<Type> Tools => Enumerable.Empty<Type>();

        protected override Controller CreateLeftController()
        {
            return OneHandedController.Create(this);
        }

        protected override Controller CreateRightController()
        {
            return OneHandedController.Create(this);
        }

        //public override void Impersonate(IActor actor, ImpersonationMode mode)
        //{
        //    base.Impersonate(actor, mode);

        //    var target = actor.Eyes;
        //    var origin = VR.Camera.Origin;
        //    var cam = VR.Camera.transform;

        //    origin.rotation = target.rotation * Quaternion.Inverse(cam.localRotation);
        //    origin.position += (target.position - cam.position);
        //}

        public override void MoveToPosition(Vector3 targetPosition, Quaternion rotation = default(Quaternion), bool ignoreHeight = true)
        {
            var origin = VR.Camera.Origin;
            var cam = VR.Camera.transform;

            origin.rotation = rotation * Quaternion.Inverse(cam.localRotation);
            origin.position += (targetPosition - cam.position);
        }

        protected override void OnStart()
        {
            base.OnStart();

            _GUI = GUIQuad.Create();
            _GUI.transform.parent = VR.Camera.Origin;
            _GUI.transform.localScale = Vector3.one * .3f;
            _GUI.IsOwned = true;

            _GUI.gameObject.SetActive(false);
        }


        protected override void InitializeTools(Controller controller, bool isLeft)
        {
        }

        List<CameraPreset> _presets = new List<CameraPreset>();
            
        public void ShowPresets()
        {
            foreach(var actor in VR.Interpreter.Actors)
            {
                VRLog.Info("Create for " + actor);
                _presets.Add(ImpersonationCameraPreset.Create(actor));
            }

            foreach(var t in VR.Interpreter.FindInterestingTransforms())
            {
                for(int i = 0; i < 6; i++)
                {
                    var angle = Mathf.PI / 3 * i;
                    var pos = new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle)) * VR.Settings.IPDScale;

                    for (int j = 1; j < 5; j++)
                    {

                        _presets.Add(AnchoredCameraPreset.Create(t, pos * j * 2, j * 0.5f));
                    }
                }
            }
        }

        public void HidePresets()
        {
            foreach(var selected in _presets.Where(p => p.Selected).Take(1))
            {
                selected.Apply();
                Stop();
            }

            foreach(var preset in _presets)
            {
                VRLog.Info("Delete preset");
                GameObject.Destroy(preset.gameObject);
            }

            _presets.Clear();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            // Update position
            if (Velocity != Vector3.zero)
            {
                VR.Camera.Origin.position += Velocity * Time.deltaTime;
            }
        }

        public void Stop()
        {
            Velocity = Vector3.zero;
        }

        protected override void OnLevel(int level)
        {
            base.OnLevel(level);

            MoveToPosition(Camera.main.transform.position, true);
        }

        public void ShowGUI(Controller controller)
        {
            _GUI.transform.position = controller.transform.TransformPoint(new Vector3(0, 0.05f, -0.06f));
            _GUI.transform.rotation = controller.transform.rotation * Quaternion.Euler(90.0f, 0.0f, 0.0f);
            _GUI.gameObject.SetActive(true);
        }

        internal void HideGUI()
        {
            _GUI.gameObject.SetActive(false);

        }

    }
}

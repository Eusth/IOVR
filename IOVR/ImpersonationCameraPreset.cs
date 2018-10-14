using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VRGIN.Core;

namespace IOVR
{
    class ImpersonationCameraPreset : CameraPreset
    {
        public IActor Actor;
        private OneHandedController _controller;

        private Transform _eyeTransform;
        public static ImpersonationCameraPreset Create(IActor actor)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.localScale *= 0.1f * VR.Settings.IPDScale;
            var preset = go.AddComponent<ImpersonationCameraPreset>();
            preset.Actor = actor;
            preset._eyeTransform = actor.Eyes;

            return preset;
        }


        protected override void OnUpdate()
        {
            base.OnUpdate();

            transform.position = _eyeTransform.position;
        }

        public override void Apply()
        {
            VR.Mode.Impersonate(Actor, VRGIN.Modes.ImpersonationMode.Exactly);
        }
    }
}

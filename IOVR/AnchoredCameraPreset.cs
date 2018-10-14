using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VRGIN.Core;
using VRGIN.Helpers;

namespace IOVR
{
    class AnchoredCameraPreset : CameraPreset
    {
        public Transform Anchor { get; private set; }
        public Vector3 Offset { get; private set; }

        public static CameraPreset Create(Transform anchor, Vector3 offset, float scale)
        {
            var go = UnityHelper.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.localScale *= 0.1f * scale  * VR.Settings.IPDScale;
            var preset = go.AddComponent<AnchoredCameraPreset>();
            preset.Anchor = anchor;
            preset.Offset = offset;
            return preset;
        }


        protected override void OnUpdate()
        {
            base.OnUpdate();

            transform.position = Anchor.TransformPoint(Offset);
        }

        public override void Apply()
        {
            VR.Mode.MoveToPosition(transform.position, Quaternion.LookRotation((Anchor.position - transform.position).normalized, transform.up));
        }
    }
}

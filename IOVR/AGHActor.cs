using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using VRGIN.Core;
using VRGIN.Helpers;

namespace AGHVR
{
    class AGHActor : ProtectedBehaviour, IActor
    {
        class LookAtMe : ProtectedBehaviour
        {
            // Forward = up
            // -Up = forward
            AGHActor _Actor;
            Transform _LeftHitomi;
            Transform _RightHitomi;

            Transform _LeftOrigin;
            Transform _RightOrigin;

            private float Radius = 0.08f;
            private bool _Initialized;

            protected override void OnStart()
            {
                base.OnStart();
                _Actor = GetComponent<AGHActor>();
                _LeftHitomi = (_Actor._Head.FindDescendant(new Regex("HitomiL")));
                _RightHitomi = (_Actor._Head.FindDescendant(new Regex("HitomiR")));
                
                if (!_LeftHitomi || !_RightHitomi)
                {
                    VRLog.Info("Hitomi Fail! " + name);
                    DestroyImmediate(this);
                }
            }

            private Transform CreateOrigin(Transform hitomi)
            {
                var forward = -hitomi.up;
                var origin = hitomi.position - forward * Radius;

                var wrapper = new GameObject("hitomi_origin");
                wrapper.transform.SetParent(transform, false);
                wrapper.transform.position = origin;

                VRLog.Info("C {0} -> {1}", hitomi.position, wrapper.transform.position);

                return wrapper.transform;
            }

            protected override void OnLateUpdate()
            {
                base.OnLateUpdate();

                if(!_Initialized)
                {
                    Init();
                }

                UpdateHitomi(_LeftHitomi, _LeftOrigin);
                UpdateHitomi(_RightHitomi, _RightOrigin);

            }

            private void Init()
            {
                _Initialized = true;
                _LeftOrigin = CreateOrigin(_LeftHitomi);
                _RightOrigin = CreateOrigin(_RightHitomi);
            }

            private void UpdateHitomi(Transform hitomi, Transform origin)
            {
                var camera = VR.Camera.SteamCam.head;
                var direction = (camera.position - transform.position).normalized;
                origin.rotation = Quaternion.LookRotation((camera.position + direction - origin.position).normalized, origin.up);
                //origin.LookAt(VR.Camera.transform.position);
                hitomi.position = origin.position + origin.forward * Radius;
                hitomi.rotation = Quaternion.LookRotation(origin.forward, origin.up) * Quaternion.Inverse(Quaternion.LookRotation(-Vector3.up, Vector3.forward));
            }
        }

        private bool _Enabled = false;
        private Transform _Head;
        private Transform _HeadNub;
        private float _Offset;
        private Renderer[] _FaceRenderers;
        private static readonly string[] FACE_NAMES = { "_EYE", "_face", "_faceLine", "_HB", "_Hitomi", "_HS00", "_mayu", "_mouthIN", "_WhiteEye" };
        public static AGHActor Create(Transform headNub)
        {
            var actor = new GameObject("Actor " + headNub.name).AddComponent<AGHActor>();
            actor.Init(headNub);
            return actor;
        }

        private void Init(Transform headNub)
        {
            _Head = headNub.parent;
            _HeadNub = headNub;

            _Offset = Vector3.Distance(headNub.position, _Head.position) * 0.6f;
            
            var root = headNub.Ancestors().Last();
            _FaceRenderers = SearchForFaceRenderers(root, _Head);

            if (_FaceRenderers.Count() == 0) Destroy(this);

            if ((VR.Settings as AGHSettings).LookAtMe)
            {
                OnUpdate();
                gameObject.AddComponent<LookAtMe>();
            }
        }
        
        private static Renderer[] SearchForFaceRenderers(Transform parent, Transform head)
        {

            var allBones = head.Descendants().Concat(new Transform[] { head });

            //return parent.Children().Select(c => c.GetComponent<Renderer>()).Where(c => c != null && c.gameObject.activeSelf && FACE_NAMES.Any(name => c.name.Contains(name))).ToArray();
            return parent.GetComponentsInChildren<SkinnedMeshRenderer>().Where(r => {
                var intersections = r.bones.Intersect(allBones);
                var weights = intersections.Count() > 0 ? r.sharedMesh.boneWeights : new BoneWeight[0];
                foreach(var intersection in intersections)
                {
                    int index = Array.IndexOf(r.bones, intersection);
                    var weightCount = weights.Select(w => w.boneIndex0 == index ? w.weight0
                                                       : w.boneIndex1 == index ? w.weight1
                                                       : w.boneIndex2 == index ? w.weight2
                                                       : w.boneIndex3 == index ? w.weight3
                                                       : 0).Where(w => w > 0).Count();
                    var ratio = weightCount / (float)weights.Count();
                    if (ratio > 0.1f) return true;
                }

                return FACE_NAMES.Contains(r.name);
            }).ToArray();
        }

        void OnDisable()
        {
            IsValid = false;
        }

        void OnEnable()
        {
            IsValid = true;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (_Head)
            {
                transform.rotation = Quaternion.LookRotation(_HeadNub.up, -_HeadNub.right);
                transform.position = _Head.position + _Offset * transform.up + _Offset * 0.3f * transform.forward;
                //VRGIN.Helpers.UnityHelper.DrawDebugBall(transform);
            }
        }

        public Transform Eyes
        {
            get
            {
                return transform;
            }
        }

        public bool HasHead
        {
            get
            {
                return _Enabled;
            }

            set
            {
                VRLog.Info(name + " - " + value);
                _Enabled = value;
                foreach(var part in _FaceRenderers)
                {
                    part.enabled = _Enabled;
                }
            }
        }

        public bool IsValid
        {
            get; set;
        }
    }
}

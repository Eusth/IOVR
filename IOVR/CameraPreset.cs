using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VRGIN.Core;

namespace IOVR
{
    class CameraPreset : ProtectedBehaviour
    {
        private MeshRenderer _renderer;
        private float _targetScale = 1.0f;
        private Vector3 _initialScale = Vector3.one;
        private float _scaleSpeed = 1.0f;
        
        protected override void OnStart()
        {
            base.OnStart();
            GetComponent<Collider>().isTrigger = true;
            _renderer = GetComponent<MeshRenderer>();
            _renderer.material = new Material(VR.Context.Materials.StandardShader);

            _initialScale = transform.localScale;
        }

        public static CameraPreset Create(Vector3 position, Quaternion rotation)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.localScale *= 0.1f * VR.Settings.IPDScale;
            var preset = go.AddComponent<ImpersonationCameraPreset>();
            return preset;
        }

        private bool _selected;
        public bool Selected {
            get
            {
                return _selected;
            }

            set
            {
                _selected = value;
                _renderer.material.SetColor("_Color", Selected ? Color.red : Color.white);
                
                if(_selected)
                {
                    _targetScale = 1.2f;
                    _scaleSpeed = 5.0f;
                } else
                {
                    _targetScale = 1.0f;
                    _scaleSpeed = 10.0f;
                }
            }
        }
        
        protected override void OnUpdate()
        {
            base.OnUpdate();

            transform.localScale = Vector3.Lerp(transform.localScale, _initialScale * _targetScale, Time.deltaTime * _scaleSpeed);
        }

        public virtual void Apply()
        {

        }
    }
}

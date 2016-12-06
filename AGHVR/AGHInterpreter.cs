using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VRGIN.Core;
using VRGIN.Helpers;
using VRGIN.Visuals;

namespace AGHVR
{
    class AGHInterpreter : GameInterpreter
    {
        Ho_Question _Question;
        UICamera _UICamera;
        Camera _DummyCam;
        public override IEnumerable<IActor> Actors
        {
            get
            {
                return new IActor[0];
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            SetUpFirstLevel();
        }

        void SetUpFirstLevel()
        {
            StartCoroutine(PushBack());

        }

        public override Camera FindCamera()
        {
            return Camera.main;
        }

        public override IEnumerable<Camera> FindSubCameras()
        {
            return GameObject.FindGameObjectsWithTag("MainCamera").Select(m => m.GetComponent<Camera>()).Except(new Camera[] { Camera.main });
        }

        protected override void OnLevel(int level)
        {
            base.OnLevel(level);

            VRLog.Info(level);

            if(level == 0)
            {
                SetUpFirstLevel();
            }
            if(level == 6)
            {
                var cam = GameObject.Find("CU_Camera").GetComponent<Camera>();
                cam.targetTexture = VR.GUI.uGuiTexture;
                cam.depth = 10;

                VR.Camera.GetComponent<Camera>().cullingMask |= LayerMask.GetMask("CH00", "CH01", "CH02", "PC", "Light", "BG", "Mob", "LB02", "LB03");
            }
            //if(level == 7)
            //{
            //    VR.Camera.GetComponent<Camera>().cullingMask = 0;
            //    VR.Camera.Copy(null);
            //}

            if (Camera.main.GetComponent<UICamera>())
            {
                var uiCamera = Camera.main.GetComponent<UICamera>();
                _DummyCam = new GameObject().AddComponent<Camera>();
                //_DummyCam.cullingMask = 0;

                _DummyCam.enabled = false;
                _UICamera = _DummyCam.gameObject.CopyComponentFrom(uiCamera);
                //_UICamera.rangeDistance = 50f;
                uiCamera.enabled = false;
            }

            if (GameObject.FindObjectOfType<CursorSet>())
            {
                var cursorSet = GameObject.FindObjectOfType<CursorSet>();
                new GameObject().CopyComponentFrom<CursorSet, MyCursorSet>(cursorSet);
                GameObject.Destroy(cursorSet);
            }
            
        }

        private IEnumerator PushBack()
        {
            yield return new WaitForSeconds(0.1f);

            if (GameObject.Find("Camera_UI"))
            {

                var cam = GameObject.Find("Camera_UI").GetComponent<Camera>();
                cam.targetTexture = VR.GUI.uGuiTexture;
                cam.depth = 10;

                var cullingMask = ~(LayerMask.GetMask(VR.Context.UILayer, VR.Context.InvisibleLayer));
                cullingMask |= LayerMask.GetMask("Default");

                VR.Camera.GetComponent<Camera>().cullingMask &= (~LayerMask.GetMask("NGUI_UI"));
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            var quad = GUIQuadRegistry.Quads.FirstOrDefault();
            if (quad)
            {
                var pos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
                var points = quad.GetComponent<MeshFilter>().sharedMesh.GetMappedPoints(pos);
                if (points.Length == 1 && _DummyCam)
                {
                    //UnityHelper.DrawRay(Color.red, VR.Camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition));
                    var point = quad.transform.TransformPoint(points[0]);
                    _DummyCam.transform.position = VR.Camera.transform.position;
                    
                    var ray = new Ray(VR.Camera.transform.position, (point - VR.Camera.transform.position).normalized);
                    var dameRay = _DummyCam.ScreenPointToRay(Input.mousePosition);

                    _DummyCam.transform.rotation = Quaternion.FromToRotation(dameRay.direction, ray.direction) * _DummyCam.transform.rotation;
                }
            }
        }
        
    }
}

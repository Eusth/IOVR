using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        GUIQuad _BGDisplay;

        private IEnumerable<IActor> _Actors = new AGHActor[0];

        public override IEnumerable<IActor> Actors
        {
            get
            {
                return _Actors;
            }
        }

        //public override bool IsBody(Collider collider)
        //{
        //    VRLog.Info("Is Body? {0} {1}", collider.name, LayerMask.LayerToName(collider.gameObject.layer));
        //    return collider.gameObject.layer > 0;
        //}

        protected override void OnStart()
        {
            base.OnStart();
            SetUpFirstLevel();

            var bgGrabber = new ScreenGrabber(1280, 720, ScreenGrabber.FromList(
                "Camera_BG",   // backgrounds
                "Camera_Main", // no idea
                "Camera_Effect", // effects (e.g. vignette?)
                "Camera"       // cinematics
            ));
            _BGDisplay = GUIQuad.Create(bgGrabber);
            _BGDisplay.transform.localScale = Vector3.one * 15;

            DontDestroyOnLoad(_BGDisplay.gameObject);

            _BGDisplay.gameObject.SetActive(false);
            VR.GUI.AddGrabber(bgGrabber);
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

            var scene = SceneManager.GetActiveScene();

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

            if(scene.name == "ADV" || scene.name == "RI_Home01")
            {
                float verticalOffset = scene.name == "ADV" ? -6f : 0;
                VR.Mode.MoveToPosition(Camera.main.transform.position, Camera.main.transform.rotation, false);
                VR.Camera.Origin.position = new Vector3(VR.Camera.Origin.position.x, verticalOffset, VR.Camera.Origin.position.z);

                _BGDisplay.gameObject.SetActive(true);
                _BGDisplay.transform.position = VR.Camera.transform.position + Vector3.ProjectOnPlane(VR.Camera.transform.forward, Vector3.up).normalized * 20;
                _BGDisplay.transform.rotation = Quaternion.LookRotation(_BGDisplay.transform.position - VR.Camera.Head.position);

            } else
            {
                VR.Camera.Origin.position = new Vector3(VR.Camera.Origin.position.x, 0, VR.Camera.Origin.position.z);
                _BGDisplay.gameObject.SetActive(false);
            }

            // Notify warp tools
            VR.Mode.Left.SendMessage("OnPlayAreaChanged", SendMessageOptions.DontRequireReceiver);
            VR.Mode.Right.SendMessage("OnPlayAreaChanged", SendMessageOptions.DontRequireReceiver);

            VRLog.Info("Entering Scene: {0}", SceneManager.GetActiveScene().name);
            //if(level == 7)
            //{
            //    VR.Camera.GetComponent<Camera>().cullingMask = 0;
            //    VR.Camera.Copy(null);
            //}

            if (GameObject.FindObjectOfType<CursorSet>())
            {
                var cursorSet = GameObject.FindObjectOfType<CursorSet>();
                new GameObject().CopyComponentFrom<CursorSet, MyCursorSet>(cursorSet);
                GameObject.Destroy(cursorSet);
            }

            UpdateActors();
        }
        
        private void UpdateActors()
        {
            StartCoroutine(UpdateActorsCoroutine());
        }

        private IEnumerator UpdateActorsCoroutine()
        {
            _Actors = new IActor[0];
            yield return new WaitForSeconds(1f);
            _Actors = GameObject.FindObjectsOfType<Transform>().Where(t => t.name.Contains("HeadNub") && t.transform.position.magnitude < 40f).Select(headNub => AGHActor.Create(headNub)).ToArray();
            VRLog.Info(_Actors.Count() + " Actors found");
            foreach(var actor in _Actors.OfType<AGHActor>())
            {
                VRLog.Info(actor.name);
            }
        }

        private void CleanActors()
        {
            _Actors = _Actors.Where(a => a != null && a.IsValid).ToArray();
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

            CleanActors();
        }
        
    }
}

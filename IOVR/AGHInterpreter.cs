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
    class CameraDefinition
    {
        internal Vector3 LookDirection;
        internal Vector3 Position;
        internal CameraDefinition(Vector3 position, Vector3 lookDirection)
        {
            this.LookDirection = lookDirection;
            this.Position = position;
        }

    }
    class AGHInterpreter : GameInterpreter
    {
        Ho_Question _Question;
        UICamera _UICamera;
        Camera _DummyCam;
        GUIQuad _BGDisplay;
        BGSetup _CurrentBG;

        private IEnumerable<IActor> _Actors = new AGHActor[0];
        private static IDictionary<string, CameraDefinition> _CameraPositions = new Dictionary<string, CameraDefinition>()
        {
            { "ADV", new CameraDefinition(new Vector3(0, 0, -18.0f), Vector3.forward) },
            { "CO", new CameraDefinition(new Vector3(0.0f, 4.6f, -9.0f), -Vector3.forward) },
            { "HO", new CameraDefinition(new Vector3(0.0f, 4.5f, -3.7f), -Vector3.forward) },
            { "RI", new CameraDefinition(new Vector3(0.9f, 4.9f, 11.1f), -Vector3.forward) },
            { "FH", new CameraDefinition(new Vector3(0.1f, 4.3f, 11.7f), new Vector3(-0.5f, -0.2f, -0.8f)) },
            { "CU", new CameraDefinition(new Vector3(0.2f, 5.6f, 5.4f), -Vector3.forward) },
            { "HO_Home", new CameraDefinition(new Vector3(2.8f, 4.7f, -3.0f), -Vector3.forward) },
            { "RI_Home01", new CameraDefinition(new Vector3(-0.5f, 6.3f, -3.8f), Vector3.forward) },
            { "RI_Home02", new CameraDefinition(new Vector3(-0.3f, 6.0f, -2.9f), -Vector3.forward) },
            { "HOME01", new CameraDefinition(new Vector3(0.0f, 4.5f, -7.1f), -Vector3.forward) },
            { "HOME_Elena", new CameraDefinition(new Vector3(0.8f, 4.7f, -6.6f), -Vector3.forward) },
        };

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
            //VR.GUI.AddGrabber(new CameraConsumer());
            VR.GUI.AddGrabber(bgGrabber);

            Invoke(() => OnLevel(SceneManager.GetActiveScene().buildIndex), 0.1f);
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
            VRLog.Info("Entering Scene: {0}", scene.name);

            if (level == 6)
            {
                //var cam = GameObject.Find("CU_Camera").GetComponent<Camera>();
                //cam.targetTexture = VR.GUI.uGuiTexture;
                //cam.depth = 10;

                //VR.Camera.GetComponent<Camera>().cullingMask |= LayerMask.GetMask("CH00", "CH01", "CH02", "PC", "Light", "BG", "Mob", "LB02", "LB03");
            }

            AcquireBG();
            if(!_CurrentBG)
            {
                var cam = VR.Camera.GetComponent<Camera>();
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = Color.black;
                //    StartCoroutine(LoadBG("CO"));
            }

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

        private IEnumerator LoadBG(string name)
        {
            VRLog.Info("Loading BG from other scene ({0})", name);

            SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            var scene = SceneManager.GetSceneAt(1);

            while(!scene.isLoaded)
            {
                yield return null;
            }

            var bg = scene.GetRootGameObjects().FirstOrDefault(o => o.name == "BG00");
            SceneManager.MoveGameObjectToScene(bg, SceneManager.GetActiveScene());
            SceneManager.UnloadSceneAsync(name);

            VRLog.Info("Done loading BG from other scene ({0})", name);

            AcquireBG();
        }

        private void UpdateActors()
        {
            StartCoroutine(UpdateActorsCoroutine());
        }

        private IEnumerator UpdateActorsCoroutine()
        {
            _Actors = new IActor[0];
            if (SceneManager.GetActiveScene().name == "ADV") yield break;

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

                VR.Camera.GetComponent<Camera>().cullingMask &= (~LayerMask.GetMask("NGUI_UI"));
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            CleanActors();

            if(_CurrentBG && !AnyBGSet())
            {
                VRLog.Info("Set BG");
                _CurrentBG.BGSET00();
                //VRLog.Info("BG: {0}", _CurrentBG.BGint);
            }
        }

        private void AcquireBG()
        {
            _CurrentBG = GameObject.FindObjectOfType<BGSetup>();
            if(_CurrentBG)
            {
                // Reposition camera!
                var scene = SceneManager.GetActiveScene();
                float floorHeight = _CurrentBG.transform.FindChild("BG00_A_floorA").position.y;

                CameraDefinition cameraDefinition;
                if(_CameraPositions.TryGetValue(scene.name, out cameraDefinition))
                {
                    VR.Mode.MoveToPosition(cameraDefinition.Position, Quaternion.LookRotation(cameraDefinition.LookDirection, Vector3.up), true);
                    VR.Camera.Origin.position = new Vector3(VR.Camera.Origin.position.x, floorHeight, VR.Camera.Origin.position.z);
                }
                else {
                    VR.Mode.MoveToPosition(Camera.main.transform.position, Camera.main.transform.rotation, false);
                    VR.Camera.Origin.position = new Vector3(VR.Camera.Origin.position.x, floorHeight, VR.Camera.Origin.position.z);
                }

                _BGDisplay.transform.position = new Vector3(0, floorHeight + 7f, 15f); // VR.Camera.transform.position + Vector3.ProjectOnPlane(VR.Camera.transform.forward, Vector3.up).normalized * 20;
                _BGDisplay.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

                if (scene.name == "ADV")
                {
                    _BGDisplay.gameObject.SetActive(true);
                }
                else
                {
                    _BGDisplay.gameObject.SetActive(false);
                }

                // Notify warp tools
                VR.Mode.Left.SendMessage("OnPlayAreaChanged", SendMessageOptions.DontRequireReceiver);
                VR.Mode.Right.SendMessage("OnPlayAreaChanged", SendMessageOptions.DontRequireReceiver);
            }
        }

        //private bool AnyBGSet()
        //{
        //    return NotNullAndActive(_CurrentBG.BG02) || NotNullAndActive(_CurrentBG.BG03) || 
        //}

        private bool AnyBGSet()
        {
            return  AnyChildrenActive(_CurrentBG.gameObject) || NotNullAndActive(_CurrentBG.BG03) || NotNullAndActive(_CurrentBG.BG02) || NotNullAndActive(_CurrentBG.BG04);
        }

        private bool NotNullAndActive(GameObject obj)
        {
            return obj && obj.activeSelf;
        }

        private bool AnyChildrenActive(GameObject obj)
        {
            return NotNullAndActive(obj) && obj.Children().Any(c => c && c.activeSelf);
        }

        public override int DefaultCullingMask
        {
            get
            {
                int level = SceneManager.GetActiveScene().buildIndex;
                int cullingMask = base.DefaultCullingMask;

                switch(level)
                {
                    case 0:
                    case 6:
                        cullingMask |= LayerMask.GetMask("CH00", "CH01", "CH02", "PC", "Light", "BG", "Mob", "LB02", "LB03");
                        break;
                }

                return cullingMask;
            }
        }
    }
}

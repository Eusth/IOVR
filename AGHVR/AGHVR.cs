using IllusionPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using VRGIN.Core;
using VRGIN.Helpers;
using VRGIN.Modes;

namespace AGHVR
{
    public class AGHVR : IEnhancedPlugin
    {
        public string[] Filter
        {
            get
            {
                return new string[] { "AGH" };
            }
        }

        public string Name
        {
            get
            {
                return "AGHVR";
            }
        }

        public string Version
        {
            get
            {
                return "0.0";
            }
        }

        public void OnApplicationQuit()
        {
        }
        
        public void OnApplicationStart()
        {
            if (Environment.CommandLine.Contains("--vr"))
            {
                var context = new AGHContext();
                VRManager.Create<AGHInterpreter>(context);
                VR.Manager.SetMode<AGHSeatedMode>();
            }
            //VRLog.Info("Layers: " + string.Join(", ", UnityHelper.GetLayerNames(int.MaxValue)));
            //UnityEngine.SceneManagement.SceneManager.LoadScene(7);
        }

        public void OnFixedUpdate()
        {
        }

        public void OnLateUpdate()
        {
        }

        public void OnLevelWasInitialized(int level)
        {
            //VRLog.Info(string.Join(", ", GameObject.FindObjectsOfType<Renderer>().Select(r => r.name + ": "+ string.Join(" - ", r.sharedMaterials.Select(m => m.name).ToArray()) ).Distinct().ToArray()));
        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnUpdate()
        {
            

            if (Input.GetKeyUp(KeyCode.Space))
            {
              
                //foreach(var face in GameObject.FindObjectsOfType<SkinnedMeshRenderer>())//.Where(t =>t.name.EndsWith("_face")))
                //{
                //    //face.gameObject.SetActive(false);
                //    VRLog.Info(face.name + " : " + string.Join(", ", face.bones.Select(b => b.name).ToArray()));
                //}
                //var pattern = new Regex("eye", RegexOptions.IgnoreCase);
                //foreach (var animator in GameObject.FindObjectsOfType<Animator>())
                //{
                //    var eye = animator.transform.FindDescendant(pattern);
                //    if(eye)
                //    {
                //        UnityHelper.DrawDebugBall(eye);
                //    } else
                //    {
                //        VRLog.Info("No eye found: " + animator.name);
                //    }

                //    //VRLog.Info(animator.name + " : " + animator.isHuman + " : " + (bone ? bone.position : Vector3.down));
                //}

                //foreach(var face in GameObject.FindObjectsOfType<Transform>().Where(t => t.name.Contains("Head") && t.name.Contains("Nub")))
                //{
                //    VRLog.Info(face.name);
                //    UnityHelper.DrawDebugBall(face);

                //    UnityHelper.DrawRay(UnityEngine.Random.ColorHSV(), face.parent.parent.position, face.parent.parent.forward);
                //}

                foreach (var actor in VR.Interpreter.Actors)
                {
                    UnityHelper.DrawRay(UnityEngine.Random.ColorHSV(), actor.Eyes.position, actor.Eyes.up);
                }
            }
        }
    }
}

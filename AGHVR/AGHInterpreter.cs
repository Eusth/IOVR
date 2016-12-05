using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VRGIN.Core;
using VRGIN.Helpers;

namespace AGHVR
{
    class AGHInterpreter : GameInterpreter
    {
        Ho_Question _Question;
        UICamera _UICamera;
        public override IEnumerable<IActor> Actors
        {
            get
            {
                return new IActor[0];
            }
        }

        protected override void OnLevel(int level)
        {
            base.OnLevel(level);

            foreach (var src in GameObject.FindObjectsOfType<AudioSource>())
            {
                src.spatialBlend = 1;
                src.spatialize = true;
            }

            var existingUiCamera = VR.Camera.gameObject.GetComponent<UICamera>();
            if(existingUiCamera)
            {
                DestroyImmediate(existingUiCamera);
            }
            if (Camera.main.GetComponent<UICamera>())
            {
                var uiCamera = Camera.main.GetComponent<UICamera>();
                VR.Camera.gameObject.CopyComponentFrom<UICamera>(uiCamera);
                uiCamera.enabled = false;
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();


            
            if (_Question)
            {
                if(_Question.QuestionNow)
                {
                    if (Input.GetKeyUp(KeyCode.F1))
                    {
                        {
                            _Question.QuestionSelect01();
                        }
                    }
                    if (Input.GetKeyUp(KeyCode.F2))
                    {
                        {
                            _Question.QuestionSelect02();
                        }
                    }
                    if (Input.GetKeyUp(KeyCode.F3))
                    {
                        {
                            _Question.QuestionSelect03();
                        }
                    }
                } else if(_Question.Icon.active)
                {
                    if (Input.GetKeyUp(KeyCode.F1))
                    {
                        _Question.IEQuestionStart();
                    }
                }
            } else
            {
                _Question = GameObject.FindObjectOfType<Ho_Question>();
            }
        }
    }
}

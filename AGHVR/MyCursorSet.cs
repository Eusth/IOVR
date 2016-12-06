using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRGIN.Core;

namespace AGHVR
{
    public class MyCursorSet : CursorSet
    {
        public override void Start()
        {
            if (GameObject.Find("UI Root(UI)/Camera") != null)
            {
                this.UICamera = GameObject.Find("UI Root(UI)/Camera").GetComponent<Camera>();
            }
            else
            {
                this.UICamera = GameObject.Find("UI Root(UI)/Camera_UI").GetComponent<Camera>();
            }
            if (SceneManager.GetActiveScene().name != "Title")
            {
                this.MainCamera = GameObject.Find("Camera_Main").GetComponent<Camera>();
            }
            if (SceneManager.GetActiveScene().name == "CU")
            {
                this.CU_Camera = GameObject.Find("UI Root(CU)/CU_Camera").GetComponent<Camera>();
            }
            SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        private void SetCursor(Texture2D texture, Vector3 hotspot, CursorMode mode)
        {
            if(VR.GUI.SoftCursor)
            {
                VR.GUI.SoftCursor.SetCursor(texture ?? this.Cursor00);
            }
        }

        public override void Update()
        {
            if (!this.On)
            {
                return;
            }
            this.UIray = this.UICamera.ScreenPointToRay(Input.mousePosition);
            if (SceneManager.GetActiveScene().name != "Title")
            {
                this.Mainray = this.MainCamera.ScreenPointToRay(Input.mousePosition);
            }
            if (SceneManager.GetActiveScene().name == "CU")
            {
                this.CUray = this.CU_Camera.ScreenPointToRay(Input.mousePosition);
            }
            if (Physics.Raycast(this.Mainray, out this.Hit))
            {
                if (this.Hit.collider.gameObject.CompareTag("material"))
                {
                    this.hotSpot = new Vector2(6f, 0f);
                    SetCursor(this.material, this.hotSpot, CursorMode.ForceSoftware);
                    this.Mainray_Hit = true;
                }
                else if (this.Hit.collider.gameObject.CompareTag("PushBt"))
                {
                    this.hotSpot = new Vector2(6f, 0f);
                    SetCursor(this.Cursor01, this.hotSpot, CursorMode.ForceSoftware);
                    this.Mainray_Hit = true;
                }
                else if (this.Hit.collider.gameObject.CompareTag("Nade") && SaveLoad_Game.Data.savegamedata.Power > 0f && SaveLoad_Game.Data.savegamedata.root)
                {
                    this.hotSpot = new Vector2(11f, 11f);
                    SetCursor(this.Cursor07, this.hotSpot, CursorMode.ForceSoftware);
                    this.Mainray_Hit = true;
                }
                else if (this.Hit.collider.gameObject.CompareTag("Kiss"))
                {
                    this.hotSpot = new Vector2(11f, 11f);
                    SetCursor(this.Cursor07, this.hotSpot, CursorMode.ForceSoftware);
                    this.Mainray_Hit = true;
                }
                else if (this.Hit.collider.gameObject.CompareTag("Untagged"))
                {
                    this.Mainray_Hit = false;
                }
                else
                {
                    this.Mainray_Hit = false;
                }
            }
            else
            {
                this.Mainray_Hit = false;
            }
            if (Physics.Raycast(this.UIray, out this.Hit))
            {
                if (this.Hit.collider.gameObject.CompareTag("PushBt"))
                {
                    this.hotSpot = new Vector2(6f, 0f);
                    SetCursor(this.Cursor01, this.hotSpot, CursorMode.ForceSoftware);
                    this.UIray_Hit = true;
                }
                else if (this.Hit.collider.gameObject.CompareTag("CursorReSize"))
                {
                    this.hotSpot = new Vector2(0f, 0f);
                    SetCursor(this.Cursor02, this.hotSpot, CursorMode.ForceSoftware);
                    this.UIray_Hit = true;
                }
                else if (this.Hit.collider.gameObject.CompareTag("CursorMov"))
                {
                    this.hotSpot = new Vector2(11f, 11f);
                    SetCursor(this.Cursor03, this.hotSpot, CursorMode.ForceSoftware);
                    this.UIray_Hit = true;
                }
                else if (this.Hit.collider.gameObject.name == "CH01")
                {
                    this.hotSpot = new Vector2(0f, 0f);
                    SetCursor(this.Cursor04, this.hotSpot, CursorMode.ForceSoftware);
                    this.UIray_Hit = true;
                }
                else if (this.Hit.collider.gameObject.CompareTag("Untagged"))
                {
                    this.UIray_Hit = false;
                }
                else
                {
                    this.UIray_Hit = false;
                }
            }
            else
            {
                this.UIray_Hit = false;
            }
            if (!this.UIray_Hit && !this.Mainray_Hit)
            {
                this.hotSpot = new Vector2(0f, 0f);
                SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
            }
        }
    }
}

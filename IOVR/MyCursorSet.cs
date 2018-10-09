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
            base.Start();
            
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
            // Taken from original assembly
            if (!this.On)
            {
                return;
            }
            this.UIray = this.UICamera.ScreenPointToRay(Input.mousePosition);
            if (!(SceneManager.GetActiveScene().name == "Title") && !(SceneManager.GetActiveScene().name == "PointCard"))
            {
                this.Mainray = this.MainCamera.ScreenPointToRay(Input.mousePosition);
            }

            if (Physics.Raycast(this.Mainray, out this.Hit))
            {
                if (this.Hit.collider.gameObject.CompareTag("Muchi"))
                {
                    this.hotSpot = new Vector2(6f, 0f);
                    if (GameClass.WhipOrWax == 1)
                    {
                        SetCursor(this.Cursor09, this.hotSpot, 0);
                    }
                    else if (GameClass.WhipOrWax == 2)
                    {
                        SetCursor(this.Cursor10, this.hotSpot, 0);
                    }
                    this.Mainray_Hit = true;
                }
                else if (this.Hit.collider.gameObject.CompareTag("Untagged"))
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
                if (LayerMask.LayerToName(this.Hit.collider.gameObject.layer) == "UI_Danmen" && ConfigClass.Danmen && ConfigClass.DanmenScene)
                {
                    if (this.Hit.collider.gameObject.CompareTag("PushBt"))
                    {
                        this.hotSpot = new Vector2(6f, 0f);
                        SetCursor(this.Cursor01, this.hotSpot, 0);
                        this.UIray_Hit = true;
                    }
                    else if (this.Hit.collider.gameObject.CompareTag("CursorReSize"))
                    {
                        this.hotSpot = new Vector2(0f, 0f);
                        SetCursor(this.Cursor02, this.hotSpot, 0);
                        this.UIray_Hit = true;
                    }
                    else if (this.Hit.collider.gameObject.CompareTag("CursorMov"))
                    {
                        this.hotSpot = new Vector2(11f, 11f);
                        SetCursor(this.Cursor03, this.hotSpot, 0);
                        this.UIray_Hit = true;
                    }
                    else if (this.Hit.collider.gameObject.name == "CH01")
                    {
                        this.hotSpot = new Vector2(0f, 0f);
                        SetCursor(this.Cursor04, this.hotSpot, 0);
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
                if (LayerMask.LayerToName(this.Hit.collider.gameObject.layer) == "UI_KeyPad" && ConfigClass.KeyPad && ConfigClass.KeyPadScene)
                {
                    if (this.Hit.collider.gameObject.CompareTag("PushBt"))
                    {
                        this.hotSpot = new Vector2(6f, 0f);
                        SetCursor(this.Cursor01, this.hotSpot, 0);
                        this.UIray_Hit = true;
                    }
                    else if (this.Hit.collider.gameObject.CompareTag("CursorReSize"))
                    {
                        this.hotSpot = new Vector2(0f, 0f);
                        SetCursor(this.Cursor02, this.hotSpot, 0);
                        this.UIray_Hit = true;
                    }
                    else if (this.Hit.collider.gameObject.CompareTag("CursorMov"))
                    {
                        this.hotSpot = new Vector2(11f, 11f);
                        SetCursor(this.Cursor03, this.hotSpot, 0);
                        this.UIray_Hit = true;
                    }
                    else if (this.Hit.collider.gameObject.name == "CH01")
                    {
                        this.hotSpot = new Vector2(0f, 0f);
                        SetCursor(this.Cursor04, this.hotSpot, 0);
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
                if (LayerMask.LayerToName(this.Hit.collider.gameObject.layer) == "UI")
                {
                    if (this.Hit.collider.gameObject.CompareTag("PushBt"))
                    {
                        this.hotSpot = new Vector2(6f, 0f);
                        SetCursor(this.Cursor01, this.hotSpot, 0);
                        this.UIray_Hit = true;
                    }
                    else if (this.Hit.collider.gameObject.CompareTag("CursorReSize"))
                    {
                        this.hotSpot = new Vector2(0f, 0f);
                        SetCursor(this.Cursor02, this.hotSpot, 0);
                        this.UIray_Hit = true;
                    }
                    else if (this.Hit.collider.gameObject.CompareTag("CursorMov"))
                    {
                        this.hotSpot = new Vector2(11f, 11f);
                        SetCursor(this.Cursor03, this.hotSpot, 0);
                        this.UIray_Hit = true;
                    }
                    else if (this.Hit.collider.gameObject.name == "CH01")
                    {
                        this.hotSpot = new Vector2(0f, 0f);
                        SetCursor(this.Cursor04, this.hotSpot, 0);
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
            }
            else
            {
                if (GameClass.PCheadPan || SceneManager.GetActiveScene().name != "ADV")
                {
                    this.hotSpot = new Vector2(11f, 11f);
                    SetCursor(this.Cursor08, this.hotSpot, 0);
                }
                this.UIray_Hit = false;
            }
            if (!GameClass.PCheadPan && !this.UIray_Hit && !this.Mainray_Hit)
            {
                this.hotSpot = new Vector2(0f, 0f);
                SetCursor(null, Vector2.zero, 0);
            }
        }
    }
}

// Decompiled with JetBrains decompiler
// Type: HSStudioNEOAddon.ObjMoveRotAssist.ObjMoveRotAssistMgr
// Assembly: HSStudioNEOAddon, Version=0.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: C00929C9-6BA1-439E-957D-40D65F705202
// Assembly location: C:\Users\Administrator\Desktop\HSStudioNEOAddon.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Studio;
using UnityEngine;

namespace FkAssistPlugin
{
    internal class ObjMoveRotAssistMgr : MonoBehaviour
    {
        public static float MOVE_RATIO = 2.5f;
        public static float ROTATE_RATIO = 90f;
        public static float SCALE_RATIO = 0.5f;

        private static ObjMoveRotAssistMgr.KEY[] ROTATION_MODE_KEYS = new ObjMoveRotAssistMgr.KEY[6]
        {
            ObjMoveRotAssistMgr.KEY.OBJ_ROT_X, ObjMoveRotAssistMgr.KEY.OBJ_ROT_Y, ObjMoveRotAssistMgr.KEY.OBJ_ROT_Z,
            ObjMoveRotAssistMgr.KEY.OBJ_ROT_X_2, ObjMoveRotAssistMgr.KEY.OBJ_ROT_Y_2,
            ObjMoveRotAssistMgr.KEY.OBJ_ROT_Z_2
        };

        private ObjMoveRotAssistMgr.KEY mode = ObjMoveRotAssistMgr.KEY.NONE;

        private Dictionary<int, GuideObject> targets = new Dictionary<int, GuideObject>();

//    private Dictionary<ObjMoveRotAssistMgr.KEY, KeyUtil> keyUtils = new Dictionary<ObjMoveRotAssistMgr.KEY, KeyUtil>();
        private Dictionary<ObjMoveRotAssistMgr.KEY, string> DEFAULT_KEYS =
            new Dictionary<ObjMoveRotAssistMgr.KEY, string>()
            {
                {ObjMoveRotAssistMgr.KEY.OBJ_MOVE_XZ, "G"},
                {ObjMoveRotAssistMgr.KEY.OBJ_MOVE_Y, "H"},
                {ObjMoveRotAssistMgr.KEY.OBJ_ROT_X, "G"},
                {ObjMoveRotAssistMgr.KEY.OBJ_ROT_Y, "H"},
                {ObjMoveRotAssistMgr.KEY.OBJ_ROT_Z, "Y"},
                {ObjMoveRotAssistMgr.KEY.OBJ_SCALE_X, "G"},
                {ObjMoveRotAssistMgr.KEY.OBJ_SCALE_Y, "H"},
                {ObjMoveRotAssistMgr.KEY.OBJ_SCALE_Z, "Y"},
                {ObjMoveRotAssistMgr.KEY.OBJ_SCALE_ALL, "T"},
                {ObjMoveRotAssistMgr.KEY.OBJ_ROT_X_2, "Shift+G"},
                {ObjMoveRotAssistMgr.KEY.OBJ_ROT_Y_2, "Shift+H"},
                {ObjMoveRotAssistMgr.KEY.OBJ_ROT_Z_2, "Shift+Y"}
            };

        private Vector2 beginMousePos;
        private Vector2 lastMousePos;
        private Dictionary<int, Vector3> oldPos;
        private Dictionary<int, Vector3> oldRot;
        private Dictionary<int, Vector3> oldScale;
        private GuideObject firstTarget;

        private void Start()
        {
            Logger.Log("Assist Start");
//      this.InitKey();
        }

//    private void InitKey()
//    {
//      this.keyUtils = new Dictionary<ObjMoveRotAssistMgr.KEY, KeyUtil>();
//      foreach (ObjMoveRotAssistMgr.KEY key in this.DEFAULT_KEYS.Keys)
//      {
//        string keyPattern = Settings.Instance.ShortcutKey(key.ToString(), this.DEFAULT_KEYS[key]);
//        if (keyPattern.ToLower() == "<none>")
//        {
//          this.keyUtils[key] = KeyUtil.NoMatchKey();
//        }
//        else
//        {
//          KeyUtil keyUtil = KeyUtil.Parse(keyPattern) ?? KeyUtil.Parse(this.DEFAULT_KEYS[key]);
//          this.keyUtils[key] = keyUtil;
//        }
//      }
//    }

        public GuideObject GetTargetObject()
        {
            GuideObject guideObject = Singleton<GuideObjectManager>.Instance.operationTarget;
            if ((UnityEngine.Object) guideObject == (UnityEngine.Object) null)
                guideObject = Singleton<GuideObjectManager>.Instance.selectObject;
            return guideObject;
        }

        public ObjectCtrlInfo GetFirstObject()
        {
            Studio.Studio instance = Singleton<Studio.Studio>.Instance;
            if ((UnityEngine.Object) instance != (UnityEngine.Object) null)
            {
                ObjectCtrlInfo[] selectObjectCtrl = instance.treeNodeCtrl.selectObjectCtrl;
                if (selectObjectCtrl != null && selectObjectCtrl.Length != 0)
                    return selectObjectCtrl[0];
            }
            return (ObjectCtrlInfo) null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Logger.Log("Assist A");
            }
        }
        
        private void Update2()
        {
            GuideObjectManager instance = Singleton<GuideObjectManager>.Instance;
            Vector2 vector2 = this.GetMousePos() - this.lastMousePos;
            if (this.mode == ObjMoveRotAssistMgr.KEY.NONE &&
                (instance.selectObjects == null || instance.selectObjects.Length == 0))
                return;
            if (this.mode == ObjMoveRotAssistMgr.KEY.OBJ_MOVE_XZ)
            {
                this.Move(new Vector3(-vector2.x, 0.0f, -vector2.y));
                if (!this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_MOVE_XZ) || instance.mode != 0)
                {
                    this.FinishMove();
                    this.mode = ObjMoveRotAssistMgr.KEY.NONE;
                }
            }
            else if (this.mode == ObjMoveRotAssistMgr.KEY.OBJ_MOVE_Y)
            {
                this.Move(new Vector3(0.0f, vector2.y, 0.0f));
                if (!this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_MOVE_Y) || instance.mode != 0)
                {
                    this.FinishMove();
                    this.mode = ObjMoveRotAssistMgr.KEY.NONE;
                }
            }
            else if (this.mode == ObjMoveRotAssistMgr.KEY.OBJ_ROT_X)
            {
                this.Rotate(new Vector3((float) (((double) vector2.x + (double) vector2.y) / 2.0), 0.0f, 0.0f));
                if (!this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_ROT_X) || instance.mode != 1)
                {
                    this.FinishRotate();
                    this.mode = ObjMoveRotAssistMgr.KEY.NONE;
                }
            }
            else if (this.mode == ObjMoveRotAssistMgr.KEY.OBJ_ROT_Y)
            {
                this.Rotate(new Vector3(0.0f, (float) (((double) vector2.x + (double) vector2.y) / 2.0), 0.0f));
                if (!this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_ROT_Y) || instance.mode != 1)
                {
                    this.FinishRotate();
                    this.mode = ObjMoveRotAssistMgr.KEY.NONE;
                }
            }
            else if (this.mode == ObjMoveRotAssistMgr.KEY.OBJ_ROT_Z)
            {
                this.Rotate(new Vector3(0.0f, 0.0f, (float) (((double) vector2.x + (double) vector2.y) / 2.0)));
                if (!this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_ROT_Z) || instance.mode != 1)
                {
                    this.FinishRotate();
                    this.mode = ObjMoveRotAssistMgr.KEY.NONE;
                }
            }
            else if (this.mode == ObjMoveRotAssistMgr.KEY.OBJ_ROT_X_2)
            {
                this.Rotate2(Vector3.right, (float) (((double) vector2.x + (double) vector2.y) / 2.0));
                if (!this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_ROT_X_2) || instance.mode != 1)
                {
                    this.FinishRotate();
                    this.mode = ObjMoveRotAssistMgr.KEY.NONE;
                }
            }
            else if (this.mode == ObjMoveRotAssistMgr.KEY.OBJ_ROT_Y_2)
            {
                this.Rotate2(Vector3.up, (float) (((double) vector2.x + (double) vector2.y) / 2.0));
                if (!this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_ROT_Y_2) || instance.mode != 1)
                {
                    this.FinishRotate2();
                    this.mode = ObjMoveRotAssistMgr.KEY.NONE;
                }
            }
            else if (this.mode == ObjMoveRotAssistMgr.KEY.OBJ_ROT_Z_2)
            {
                this.Rotate2(Vector3.forward, (float) (((double) vector2.x + (double) vector2.y) / 2.0));
                if (!this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_ROT_Z_2) || instance.mode != 1)
                {
                    this.FinishRotate2();
                    this.mode = ObjMoveRotAssistMgr.KEY.NONE;
                }
            }
            else if (this.mode == ObjMoveRotAssistMgr.KEY.OBJ_SCALE_ALL)
            {
                Vector3 vector3 = (Vector3) (this.GetMousePos() - this.beginMousePos);
                this.Scale(Vector3.one * (float) (((double) vector3.x + (double) vector3.y) / 2.0));
                if (!this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_SCALE_ALL) || instance.mode != 2)
                {
                    this.FinishScale();
                    this.mode = ObjMoveRotAssistMgr.KEY.NONE;
                }
            }
            else if (this.mode == ObjMoveRotAssistMgr.KEY.OBJ_SCALE_X)
            {
                Vector3 vector3 = (Vector3) (this.GetMousePos() - this.beginMousePos);
                this.Scale(Vector3.left * (float) (((double) vector3.x + (double) vector3.y) / 2.0));
                if (!this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_SCALE_ALL) || instance.mode != 2)
                {
                    this.FinishScale();
                    this.mode = ObjMoveRotAssistMgr.KEY.NONE;
                }
            }
            else if (this.mode == ObjMoveRotAssistMgr.KEY.OBJ_SCALE_Y)
            {
                Vector3 vector3 = (Vector3) (this.GetMousePos() - this.beginMousePos);
                this.Scale(Vector3.up * (float) (((double) vector3.x + (double) vector3.y) / 2.0));
                if (!this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_SCALE_ALL) || instance.mode != 2)
                {
                    this.FinishScale();
                    this.mode = ObjMoveRotAssistMgr.KEY.NONE;
                }
            }
            else if (this.mode == ObjMoveRotAssistMgr.KEY.OBJ_SCALE_Z)
            {
                Vector3 vector3 = (Vector3) (this.GetMousePos() - this.beginMousePos);
                this.Scale(Vector3.forward * (float) (((double) vector3.x + (double) vector3.y) / 2.0));
                if (!this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_SCALE_ALL) || instance.mode != 2)
                {
                    this.FinishScale();
                    this.mode = ObjMoveRotAssistMgr.KEY.NONE;
                }
            }
            else if (this.mode == ObjMoveRotAssistMgr.KEY.NONE)
            {
                if (instance.mode == 0)
                {
                    if (this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_MOVE_XZ))
                    {
                        this.mode = ObjMoveRotAssistMgr.KEY.OBJ_MOVE_XZ;
                        this.oldPos = this.CollectOldPos();
                    }
                    else if (this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_MOVE_Y))
                    {
                        this.mode = ObjMoveRotAssistMgr.KEY.OBJ_MOVE_Y;
                        this.oldPos = this.CollectOldPos();
                    }
                }
                else if (instance.mode == 1)
                {
                    for (int index = 0; index < ObjMoveRotAssistMgr.ROTATION_MODE_KEYS.Length; ++index)
                    {
                        if (this.GetKey(ObjMoveRotAssistMgr.ROTATION_MODE_KEYS[index]))
                        {
                            this.mode = ObjMoveRotAssistMgr.ROTATION_MODE_KEYS[index];
                            this.firstTarget = this.GetTargetObject();
                            this.oldPos = this.CollectOldPos();
                            this.oldRot = this.CollectOldRot();
                            break;
                        }
                    }
                }
                else if (instance.mode == 2)
                {
                    if (this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_SCALE_ALL))
                    {
                        this.mode = ObjMoveRotAssistMgr.KEY.OBJ_SCALE_ALL;
                        this.oldScale = this.CollectOldScale();
                        this.beginMousePos = this.GetMousePos();
                    }
                    else if (this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_SCALE_X))
                    {
                        this.mode = ObjMoveRotAssistMgr.KEY.OBJ_SCALE_X;
                        this.oldScale = this.CollectOldScale();
                        this.beginMousePos = this.GetMousePos();
                    }
                    else if (this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_SCALE_Y))
                    {
                        this.mode = ObjMoveRotAssistMgr.KEY.OBJ_SCALE_Y;
                        this.oldScale = this.CollectOldScale();
                        this.beginMousePos = this.GetMousePos();
                    }
                    else if (this.GetKey(ObjMoveRotAssistMgr.KEY.OBJ_SCALE_Z))
                    {
                        this.mode = ObjMoveRotAssistMgr.KEY.OBJ_SCALE_Z;
                        this.oldScale = this.CollectOldScale();
                        this.beginMousePos = this.GetMousePos();
                    }
                }
            }
            else
            {
                Console.WriteLine("Unknown mode ");
                this.mode = ObjMoveRotAssistMgr.KEY.NONE;
            }
            this.lastMousePos = this.GetMousePos();
        }

        private Dictionary<int, Vector3> CollectOldPos()
        {
            Dictionary<int, Vector3> dictionary = new Dictionary<int, Vector3>();
            this.targets = new Dictionary<int, GuideObject>();
            foreach (GuideObject selectObject in Singleton<GuideObjectManager>.Instance.selectObjects)
            {
                if (selectObject.enablePos)
                {
                    dictionary.Add(selectObject.dicKey, selectObject.changeAmount.pos);
                    this.targets.Add(selectObject.dicKey, selectObject);
                }
            }
            return dictionary;
        }

        private Dictionary<int, Vector3> CollectOldRot()
        {
            Dictionary<int, Vector3> dictionary = new Dictionary<int, Vector3>();
            this.targets = new Dictionary<int, GuideObject>();
            foreach (GuideObject selectObject in Singleton<GuideObjectManager>.Instance.selectObjects)
            {
                if (selectObject.enableRot)
                {
                    dictionary.Add(selectObject.dicKey, selectObject.changeAmount.rot);
                    this.targets.Add(selectObject.dicKey, selectObject);
                }
            }
            return dictionary;
        }

        private Dictionary<int, Vector3> CollectOldScale()
        {
            Dictionary<int, Vector3> dictionary = new Dictionary<int, Vector3>();
            this.targets = new Dictionary<int, GuideObject>();
            foreach (GuideObject selectObject in Singleton<GuideObjectManager>.Instance.selectObjects)
            {
                if (selectObject.enableScale)
                {
                    dictionary.Add(selectObject.dicKey, selectObject.changeAmount.scale);
                    this.targets.Add(selectObject.dicKey, selectObject);
                }
            }
            return dictionary;
        }

        private void Move(Vector3 delta)
        {
            Camera mainCmaera = Singleton<Studio.Studio>.Instance.cameraCtrl.mainCmaera;
            if ((UnityEngine.Object) mainCmaera != (UnityEngine.Object) null)
            {
                Vector3 position = new Vector3((float) Screen.width * 0.5f, (float) Screen.height * 0.5f,
                    Input.mousePosition.z);
                Ray ray = mainCmaera.ScreenPointToRay(position);
                ray.direction = new Vector3(ray.direction.x, 0.0f, ray.direction.z);
                Vector3 vector3_1 = ray.direction * -1f * delta.z;
                ray.direction = Quaternion.LookRotation(ray.direction) * Vector3.right;
                Vector3 vector3_2 = vector3_1 + ray.direction * -1f * delta.x;
                vector3_2.y = delta.y;
                delta = vector3_2;
            }
            delta = delta * Studio.Studio.optionSystem.manipuleteSpeed * ObjMoveRotAssistMgr.MOVE_RATIO;
            foreach (GuideObject guideObject in this.targets.Values)
            {
                if (guideObject.enablePos)
                {
                    guideObject.transformTarget.position = guideObject.transformTarget.position + delta;
                    guideObject.changeAmount.pos = guideObject.transformTarget.localPosition;
                }
            }
        }

        private void FinishMove()
        {
            Singleton<UndoRedoManager>.Instance.Push((ICommand) new GuideCommand.MoveEqualsCommand(this.targets
                .Select<KeyValuePair<int, GuideObject>, GuideCommand.EqualsInfo>(
                    (System.Func<KeyValuePair<int, GuideObject>, GuideCommand.EqualsInfo>) (v =>
                        new GuideCommand.EqualsInfo()
                        {
                            dicKey = v.Key,
                            oldValue = this.oldPos[v.Key],
                            newValue = v.Value.changeAmount.pos
                        })).ToArray<GuideCommand.EqualsInfo>()));
        }

        private void Rotate(Vector3 delta)
        {
            delta = delta * Studio.Studio.optionSystem.manipuleteSpeed * ObjMoveRotAssistMgr.ROTATE_RATIO;
            foreach (GuideObject guideObject in this.targets.Values)
            {
                if (guideObject.enableRot)
                {
                    Vector3 vector3 = guideObject.transformTarget.localEulerAngles += delta;
                    guideObject.transformTarget.localEulerAngles = vector3;
                    guideObject.changeAmount.rot = guideObject.transformTarget.localEulerAngles;
                }
            }
        }

        private void FinishRotate()
        {
            GuideObjectManager instance = Singleton<GuideObjectManager>.Instance;
            Singleton<UndoRedoManager>.Instance.Push((ICommand) new GuideCommand.RotationEqualsCommand(this.targets
                .Select<KeyValuePair<int, GuideObject>, GuideCommand.EqualsInfo>(
                    (System.Func<KeyValuePair<int, GuideObject>, GuideCommand.EqualsInfo>) (v =>
                        new GuideCommand.EqualsInfo()
                        {
                            dicKey = v.Key,
                            oldValue = this.oldRot[v.Key],
                            newValue = v.Value.changeAmount.rot
                        })).ToArray<GuideCommand.EqualsInfo>()));
        }

        private void Rotate2(Vector3 axis, float delta)
        {
            delta = delta * Studio.Studio.optionSystem.manipuleteSpeed * ObjMoveRotAssistMgr.ROTATE_RATIO;
            Vector3 position = this.firstTarget.transformTarget.position;
            foreach (GuideObject guideObject in this.targets.Values)
            {
                Vector3 vector3_1 = Vector3.zero;
                Vector3 vector3_2 = Vector3.zero;
                if (!guideObject.enableRot)
                    vector3_2 = guideObject.transformTarget.localEulerAngles;
                if (!guideObject.enablePos)
                    vector3_1 = guideObject.transformTarget.localPosition;
                if (guideObject.enableRot || guideObject.enablePos)
                {
                    guideObject.transformTarget.RotateAround(position, axis, delta);
                    if (!guideObject.enablePos)
                        guideObject.transformTarget.localPosition = vector3_1;
                    if (!guideObject.enableRot)
                        guideObject.transformTarget.localEulerAngles = vector3_2;
                    guideObject.changeAmount.rot = guideObject.transformTarget.localEulerAngles;
                    guideObject.changeAmount.pos = guideObject.transformTarget.localPosition;
                }
            }
        }

        private void FinishRotate2()
        {
            GuideObjectManager instance = Singleton<GuideObjectManager>.Instance;
            Singleton<UndoRedoManager>.Instance.Push((ICommand) new ObjMoveRotAssistMgr.MoveRotEqualsCommand(this
                .targets.Select<KeyValuePair<int, GuideObject>, GuideCommand.EqualsInfo>(
                    (System.Func<KeyValuePair<int, GuideObject>, GuideCommand.EqualsInfo>) (v =>
                        new GuideCommand.EqualsInfo()
                        {
                            dicKey = v.Key,
                            oldValue = this.oldPos[v.Key],
                            newValue = v.Value.changeAmount.pos
                        })).ToArray<GuideCommand.EqualsInfo>(), this.targets
                .Select<KeyValuePair<int, GuideObject>, GuideCommand.EqualsInfo>(
                    (System.Func<KeyValuePair<int, GuideObject>, GuideCommand.EqualsInfo>) (v =>
                        new GuideCommand.EqualsInfo()
                        {
                            dicKey = v.Key,
                            oldValue = this.oldRot[v.Key],
                            newValue = v.Value.changeAmount.rot
                        })).ToArray<GuideCommand.EqualsInfo>()));
        }

        private void Scale(Vector3 scaleDelta)
        {
            Vector3 vector3_1 = scaleDelta * Studio.Studio.optionSystem.manipuleteSpeed *
                                ObjMoveRotAssistMgr.SCALE_RATIO;
            foreach (GuideObject guideObject in this.targets.Values)
            {
                if (guideObject.enableRot)
                {
                    Vector3 vector3_2 = this.oldScale[guideObject.dicKey];
                    vector3_2.x = vector3_2.x * (1f + vector3_1.x);
                    vector3_2.y = vector3_2.y * (1f + vector3_1.y);
                    vector3_2.z = vector3_2.z * (1f + vector3_1.z);
                    guideObject.transformTarget.localScale = vector3_2;
                    guideObject.changeAmount.scale = vector3_2;
                }
            }
        }

        private void FinishScale()
        {
            GuideObjectManager instance = Singleton<GuideObjectManager>.Instance;
            Singleton<UndoRedoManager>.Instance.Push((ICommand) new GuideCommand.ScaleEqualsCommand(this.targets
                .Select<KeyValuePair<int, GuideObject>, GuideCommand.EqualsInfo>(
                    (System.Func<KeyValuePair<int, GuideObject>, GuideCommand.EqualsInfo>) (v =>
                        new GuideCommand.EqualsInfo()
                        {
                            dicKey = v.Key,
                            oldValue = this.oldScale[v.Key],
                            newValue = v.Value.changeAmount.scale
                        })).ToArray<GuideCommand.EqualsInfo>()));
        }

        private Vector2 GetMousePos()
        {
            Vector2 mousePosition = (Vector2) Input.mousePosition;
            return new Vector2(mousePosition.x / (float) Screen.width, mousePosition.y / (float) Screen.height);
        }

        private bool GetKey(ObjMoveRotAssistMgr.KEY key)
        {
//      if (this.keyUtils.ContainsKey(key))
//        return this.keyUtils[key].TestKey();
            return false;
        }

        public enum KEY
        {
            OBJ_MOVE_XZ,
            OBJ_MOVE_Y,
            OBJ_ROT_X,
            OBJ_ROT_Y,
            OBJ_ROT_Z,
            OBJ_SCALE_ALL,
            OBJ_SCALE_X,
            OBJ_SCALE_Y,
            OBJ_SCALE_Z,
            OBJ_ROT_X_2,
            OBJ_ROT_Y_2,
            OBJ_ROT_Z_2,
            NONE,
        }

        public class MoveRotEqualsCommand : ICommand
        {
            private GuideCommand.EqualsInfo[] posChangeAmountInfo;
            private GuideCommand.EqualsInfo[] rotChangeAmountInfo;

            public MoveRotEqualsCommand(GuideCommand.EqualsInfo[] posChangeAmountInfo,
                GuideCommand.EqualsInfo[] rotChangeAmountInfo)
            {
                this.posChangeAmountInfo = posChangeAmountInfo;
                this.rotChangeAmountInfo = rotChangeAmountInfo;
            }

            public void Do()
            {
                for (int index = 0; index < this.posChangeAmountInfo.Length; ++index)
                {
                    ChangeAmount changeAmount = Studio.Studio.GetChangeAmount(this.posChangeAmountInfo[index].dicKey);
                    if (changeAmount != null)
                        changeAmount.pos = this.posChangeAmountInfo[index].newValue;
                }
                for (int index = 0; index < this.rotChangeAmountInfo.Length; ++index)
                {
                    ChangeAmount changeAmount = Studio.Studio.GetChangeAmount(this.rotChangeAmountInfo[index].dicKey);
                    if (changeAmount != null)
                        changeAmount.rot = this.rotChangeAmountInfo[index].newValue;
                }
            }

            public void Redo()
            {
                this.Do();
            }

            public void Undo()
            {
                for (int index = 0; index < this.posChangeAmountInfo.Length; ++index)
                {
                    ChangeAmount changeAmount = Studio.Studio.GetChangeAmount(this.posChangeAmountInfo[index].dicKey);
                    if (changeAmount != null)
                        changeAmount.pos = this.posChangeAmountInfo[index].oldValue;
                }
                for (int index = 0; index < this.rotChangeAmountInfo.Length; ++index)
                {
                    ChangeAmount changeAmount = Studio.Studio.GetChangeAmount(this.rotChangeAmountInfo[index].dicKey);
                    if (changeAmount != null)
                        changeAmount.rot = this.rotChangeAmountInfo[index].oldValue;
                }
            }
        }
    }
}
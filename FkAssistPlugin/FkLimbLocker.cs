using System;
using System.Runtime.InteropServices;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using UnityEngine;

namespace FkAssistPlugin
{
    public class FkLimbLocker : BaseMgr<FkLimbLocker>
    {
        private bool _show;
        private bool _allEnabled = true;

        private Rect _windowRect = new Rect(
            Screen.width * 0.8f,
            Screen.height * 0.6f,
            Screen.width * 0.2f,
            Screen.height * 0.4f);

        private int wid = 18539;
        //        private GameObject probeGameObject;
        //        private ReflectionProbe probeComponent;

        public override void Init()
        {
            Tracer.Log("FkLimbLocker Init");
            //            probeGameObject = new GameObject("YMLRealtimeReflectionProbe");
            //            probeComponent = probeGameObject.AddComponent<ReflectionProbe>() as ReflectionProbe;
        }

        private void OnGUI()
        {
            _windowRect = GUI.Window(wid, _windowRect, ShowWindow, "我的窗口");
        }

        Light[] GetLights()
        {
            return new Light[] { };
        }

        private void ShowWindow(int id)
        {
            try
            {
                var lights = GetLights();
                //                if (Event.current.type == EventType.MouseDown)
                //                {
                //                    GUI.FocusWindow(wid);
                //                    CameraAssist.windowdragflag = true;
                //                }
                //                else if (Event.current.type == EventType.MouseUp)
                //                {
                //                    CameraAssist.windowdragflag = false;
                //                }

                GUIX.BeginHorizontal();
                //0 Direct 1 Point 2 Spot
                if (GUIX.Button("平"))
                {
                    Singleton<Studio.Studio>.Instance.AddLight(0);
                    var obj = Context.GuideObjectManager().selectObject.transformTarget.gameObject;
                    obj.name = "D" + lights.Length;
                }
                if (GUIX.Button("点"))
                {
                    Singleton<Studio.Studio>.Instance.AddLight(1);
                    var obj = Context.GuideObjectManager().selectObject.transformTarget.gameObject;
                    obj.name = "P" + lights.Length;
                }
                if (GUIX.Button("聚"))
                {
                    Singleton<Studio.Studio>.Instance.AddLight(2);
                    var obj = Context.GuideObjectManager().selectObject.transformTarget.gameObject;
                    obj.name = "S" + lights.Length;
                }
                if (GUIX.Button("S"))
                {
                    lights.Foreach(l => { l.enabled = !l.enabled; });
                }
                if (GUIX.Button("N"))
                {
                    lights.Foreach(l => l.intensity = 0.3f);
                }
                GUIX.Label("反射");
                var refProbes = GameObject.FindObjectsOfType<ReflectionProbe>();
                if (refProbes.Length >= 1)
                {
                    var r = refProbes[0];
                    if (GUIX.Button("<"))
                    {
                        r.intensity -= 0.1f;
                    }
                    if (GUIX.Button(r.intensity.ToString("F2")))
                    {
                        r.intensity = 1f;
                    }
                    if (GUIX.Button(">"))
                    {
                        r.intensity += 0.1f;
                    }
                }
                GUIX.Label("全部");
                if (GUIX.RepeatButton("<"))
                {
                    lights.Foreach(l => l.intensity -= 0.01f);
                }
                if (GUIX.RepeatButton(">"))
                {
                    lights.Foreach(l => l.intensity += 0.01f);
                }
                //                if (GUIX.RepeatButton("<"))
                //                {
                //                    probeComponent.intensity -= 0.01f;
                //                }
                //                if (GUIX.Button(probeComponent.intensity.ToString("F2")))
                //                {
                //                    probeComponent.intensity = 1f;
                //                }
                //                if (GUIX.RepeatButton(">"))
                //                {
                //                    probeComponent.intensity += 0.01f;
                //                }
                GUIX.EndHorizontal();

                //                _scrollPos = GUILayout.BeginScrollView(_scrollPos);
                GUIX.BeginScrollView();
                lights.Foreach(l =>
                {
                    GUIX.BeginHorizontal();
                    GUIX.Label(l.name, 3);
                    l.enabled = GUIX.Toggle(l.enabled, "");
                    if (GUIX.RepeatButton("<"))
                    {
                        l.intensity -= 0.01f;
                    }
                    if (GUIX.Button(l.intensity.ToString("F2")))
                    {
                        l.intensity = 0.3f;
                    }
                    if (GUIX.RepeatButton(">"))
                    {
                        l.intensity += 0.01f;
                    }
                    if (GUIX.RepeatButton("<"))
                    {
                        l.range -= 0.1f;
                    }
                    if (GUIX.Button(l.range.ToString("F2")))
                    {
                        l.range = 10f;
                    }
                    if (GUIX.RepeatButton(">"))
                    {
                        l.range += 0.1f;
                    }
                    l.shadows = GUIX.Toggle(l.shadows != LightShadows.None, "影")
                        ? LightShadows.Soft
                        : LightShadows.None;
                    GUIX.EndHorizontal();
                });

                GUILayout.EndScrollView();
            }
            catch (Exception e)
            {
                Tracer.Log(e);
            }
        }

        private void Update()
        {
            try
            {
                InnerUpdate();
            }
            catch (Exception e)
            {
                Tracer.Log(e);
            }
        }

        private void InnerUpdate()
        {
            if (Input.GetKeyDown(KeyCode.P) && Input.GetKeyDown(KeyCode.RightShift))
            {
                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    _allEnabled = !_allEnabled;
                    GetLights().Foreach(l => l.enabled = _allEnabled);
                }
                else
                {
                    _show = !_show;
                }
            }
        }
    }
}
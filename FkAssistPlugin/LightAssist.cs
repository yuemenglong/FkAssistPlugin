using System;
using System.Collections.Generic;
using System.Security.Principal;
using FkAssistPlugin.FkBone;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using Studio;
using UnityEngine;

namespace FkAssistPlugin
{
    public class LightAssist : BaseMgr<LightAssist>
    {
        private bool _lightEnabled = true;
        private bool _show;

        private Rect _windowRect = new Rect(Screen.width * 0.5f, Screen.height * 0.64f, Screen.width * 0.33f,
            Screen.height * 0.45f);
//        private Rect _windowRect = new Rect(50, 50, 300, 500);

        private GUIStyle _windowStyle = new GUIStyle(GUI.skin.window);

        private int wid = 17539;

        public override void Init()
        {
            Tracer.Log("FkAssistPlugin Init");
        }

        private void OnGUI()
        {
            _show = GUI.Toggle(new Rect(10, 10, 100, 23), _show, "是否显示窗体");
            if (_show)
            {
                _windowRect = GUI.Window(wid, _windowRect, ShowWindow, "我的窗口");
            }
        }


        private void ShowWindow(int id)
        {
            try
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    GUI.FocusWindow(wid);
                    CameraAssist.windowdragflag = true;
                }
                else if (Event.current.type == EventType.MouseUp)
                {
                    CameraAssist.windowdragflag = false;
                }

                var lights = FindObjectsOfType(typeof(Light)) as Light[];
                var w1 = GUILayout.Width(_windowRect.width * 0.2f);
                var w2 = GUILayout.Width(_windowRect.width * 0.6f);
                lights.Foreach(l =>
                {
                    GUILayout.Label(l.name);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(l.intensity + "", w1);
                    l.intensity = GUILayout.HorizontalSlider(l.intensity, 0, 10, w2);
                    if (GUILayout.Button("<"))
                    {
                        l.intensity -= 0.1f;
                    }

                    if (GUILayout.Button("O"))
                    {
                        l.intensity = 1f;
                    }

                    if (GUILayout.Button(">"))
                    {
                        l.intensity += 0.1f;
                    }

                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(l.range + "", w1);
                    l.range = GUILayout.HorizontalSlider(l.range, 0, 100, w2);
                    if (GUILayout.Button("<"))
                    {
                        l.range -= 0.1f;
                    }

                    if (GUILayout.Button("O"))
                    {
                        l.range = 10f;
                    }


                    if (GUILayout.Button(">"))
                    {
                        l.range += 0.1f;
                    }

                    GUILayout.EndHorizontal();
                });
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
            if (Input.GetKeyDown(KeyCode.P))
            {
                Tracer.Log(Context.GuideObjectManager().selectObject.transformTarget);
                var lights = FindObjectsOfType(typeof(Light)) as Light[];
//                    var mainLight = lights.Filter(l => l.name == "MainLight")[0];
                if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                {
                    _lightEnabled = !_lightEnabled;
                    lights.Foreach(l =>
                    {
                        if (l.name != "MainLight" && l.name != "CharaLight_back")
                        {
                            l.enabled = _lightEnabled;
                        }
                    });
                }
                else
                {
                    lights.Foreach(l =>
                    {
                        var value = l.intensity;
                        if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
                        {
                            value += 0.1f;
                        }
                        else
                        {
                            value -= 0.1f;
                        }

                        l.intensity = value;
                    });
                }
            }
        }
    }
}
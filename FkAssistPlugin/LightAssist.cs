using System;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using UnityEngine;

namespace FkAssistPlugin
{
    public class LightAssist : BaseMgr<LightAssist>
    {
        private bool _show;
        private bool _allEnabled = true;

        private Rect _windowRect = new Rect(Screen.width * 0.2f, Screen.height * 0.4f, Screen.width * 0.5f,
            Screen.height * 0.59f);
//        private Rect _windowRect = new Rect(50, 50, 300, 500);

        private GUIStyle _windowStyle = new GUIStyle(GUI.skin.window);

        private int wid = 17539;
        private Vector2 _scrollPos = Vector2.zero;

        public override void Init()
        {
            Tracer.Log("FkAssistPlugin Init");
        }

        private void OnGUI()
        {
            _show = GUI.Toggle(new Rect(0, 0, 20, 20), _show, "");
            if (_show)
            {
                _windowRect = GUI.Window(wid, _windowRect, ShowWindow, "我的窗口");
            }
        }

        Light[] GetLights()
        {
            var lights = FindObjectsOfType(typeof(Light)) as Light[];
            Array.Sort(lights, (l1, l2) => String.Compare(l1.name, l2.name, StringComparison.Ordinal));
            return lights;
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

                GUI.backgroundColor = Color.gray;
                var w_5 = GUILayout.Width(_windowRect.width * 0.05f);
                var w1 = GUILayout.Width(_windowRect.width * 0.1f);
                var w2 = GUILayout.Width(_windowRect.width * 0.2f);
                var h1 = GUILayout.Height(_windowRect.height * 0.05f);
                var h2 = GUILayout.Height(_windowRect.height * 0.04f);

                var s1 = new GUIStyle();
                s1.normal.textColor = Color.white;
                s1.fontSize = (int) (_windowRect.height * 0.035);
                s1.alignment = TextAnchor.MiddleCenter;

                GUILayout.BeginHorizontal(h1);
                //0 Direct 1 Point 2 Spot
                if (GUILayout.Button("平", w_5, h2))
                {
                    Singleton<Studio.Studio>.Instance.AddLight(0);
                }
                if (GUILayout.Button("点", w_5, h2))
                {
                    Singleton<Studio.Studio>.Instance.AddLight(1);
                }
                if (GUILayout.Button("聚", w_5, h2))
                {
                    Singleton<Studio.Studio>.Instance.AddLight(2);
                }
                GUILayout.EndHorizontal();

                _scrollPos = GUILayout.BeginScrollView(_scrollPos);
                GetLights().Foreach(l =>
                {
                    GUILayout.BeginHorizontal(h1);
                    GUILayout.Label(l.name, s1, w2);
                    GUILayout.Label(l.enabled ? "O" : "", s1, w_5);
                    if (GUILayout.Button("S", w_5, h2))
                    {
                        l.enabled = !l.enabled;
                    }
//                    GUILayout.EndHorizontal();

//                    GUILayout.BeginHorizontal();
                    GUILayout.Label(l.intensity.ToString("F2"), s1, w1);
//                    l.intensity = GUILayout.HorizontalSlider(l.intensity, 0, 10, w2);
                    if (GUILayout.RepeatButton("<", w_5, h2))
                    {
                        l.intensity -= 0.01f;
                    }

                    if (GUILayout.Button("O", w_5, h2))
                    {
                        l.intensity = 1f;
                    }

                    if (GUILayout.RepeatButton(">", w_5, h2))
                    {
                        l.intensity += 0.01f;
                    }

//                    GUILayout.EndHorizontal();

//                    GUILayout.BeginHorizontal(h1);
                    GUILayout.Label(l.range.ToString("F2"), s1, w1);
//                    l.range = GUILayout.HorizontalSlider(l.range, 0, 100, w2);
                    if (GUILayout.RepeatButton("<", w_5, h2))
                    {
                        l.range -= 0.1f;
                    }

                    if (GUILayout.Button("O", w_5, h2))
                    {
                        l.range = 10f;
                    }


                    if (GUILayout.RepeatButton(">", w_5, h2))
                    {
                        l.range += 0.1f;
                    }

                    GUILayout.EndHorizontal();
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
            if (Input.GetKeyDown(KeyCode.P))
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
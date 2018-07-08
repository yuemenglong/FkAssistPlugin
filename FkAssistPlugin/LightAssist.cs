﻿using System;
using System.Runtime.InteropServices;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using UnityEngine;

namespace FkAssistPlugin
{
    public class LightAssist : BaseMgr<LightAssist>
    {
        private bool _show;
        private bool _allEnabled = true;

        private Rect _windowRect = new Rect(
            Screen.width * 0.2f,
            Screen.height * 0.4f,
            Screen.width * 0.5f,
            Screen.height * 0.59f);

        private int wid = 17539;

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

                GUIX.BeginHorizontal();
                //0 Direct 1 Point 2 Spot
                if (GUIX.Button("平"))
                {
                    Singleton<Studio.Studio>.Instance.AddLight(0);
                }
                if (GUIX.Button("点"))
                {
                    Singleton<Studio.Studio>.Instance.AddLight(1);
                }
                if (GUIX.Button("聚"))
                {
                    Singleton<Studio.Studio>.Instance.AddLight(2);
                }
                var lights = GetLights();
                if (GUIX.Button("S"))
                {
                    lights.Foreach(l => { l.enabled = !l.enabled; });
                }
                GUIX.EndHorizontal();

//                _scrollPos = GUILayout.BeginScrollView(_scrollPos);
                GUIX.BeginScrollView();
                GetLights().Foreach(l =>
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
                        l.intensity = 1f;
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
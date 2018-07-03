using System;
using System.Collections.Generic;
using FkAssistPlugin.FkBone;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using Studio;
using UnityEngine;

namespace FkAssistPlugin
{
    public class LightAssist : BaseMgr<LightAssist>
    {
        private int _counter;
        private bool _lightEnabled = true;

        public override void Init()
        {
            Tracer.Log("FkAssistPlugin Init");
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
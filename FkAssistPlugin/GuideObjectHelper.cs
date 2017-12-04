using Studio;
using UnityEngine;

namespace FkAssistPlugin
{
    public static class GuideObjectHelper
    {
        public static bool IsHand(this GuideObject go)
        {
            var name = go.transformTarget.name;
            return name == "cf_J_Hand_L"
                   || name == "cf_J_Hand_R"
                   || name == "cm_J_Hand_L"
                   || name == "cm_J_Hand_R";
        }

        public static bool IsFoot(this GuideObject go)
        {
            var name = go.transformTarget.name;
            return name == "cf_J_Foot01_L"
                   || name == "cf_J_Foot01_R"
                   || name == "cm_J_Foot01_L"
                   || name == "cm_J_Foot01_R";
        }

        public static bool IsArm(this GuideObject go)
        {
            var name = go.transformTarget.name;
            return name == "cf_J_ArmUp00_L"
                   || name == "cf_J_ArmUp00_R"
                   || name == "cm_J_ArmUp00_L"
                   || name == "cm_J_ArmUp00_R";
        }

        public static bool IsLeg(this GuideObject go)
        {
            var name = go.transformTarget.name;
            return name == "cf_J_LegUp00_L"
                   || name == "cf_J_LegUp00_R"
                   || name == "cm_J_LegUp00_L"
                   || name == "cm_J_LegUp00_R";
        }

        public static bool IsLimb(this GuideObject go)
        {
            return go.IsHand() || go.IsFoot();
        }

        public static GuideObject GuideObject(this Transform transform)
        {
            return Context.DicGuideObject()[transform];
        }

        public static void Rotate(this GuideObject guideObject, float z, float y, float x)
        {
            if (guideObject.enableRot)
            {
                guideObject.transformTarget.Rotate(z, y, x, Space.Self);
                guideObject.changeAmount.rot = guideObject.transformTarget.localEulerAngles;
            }
        }

        public static void Move(this GuideObject guideObject, float x, float y, float z)
        {
            Move(guideObject, new Vector3(x,y,z));
        }
        
        public static void Move(this GuideObject guideObject, Vector3 vec)
        {
            MoveTo(guideObject, guideObject.transformTarget.position +vec);
        }
        
        public static void MoveTo(this GuideObject guideObject, float x, float y, float z)
        {
            MoveTo(guideObject, new Vector3(x,y,z));
        } 
        
        public static void MoveTo(this GuideObject guideObject, Vector3 pos)
        {
            if (guideObject.enablePos)
            {
                guideObject.transformTarget.position = pos;
                guideObject.changeAmount.pos = guideObject.transformTarget.localPosition;
            }
        }

        public static void Reset(this GuideObject guideObject)
        {
            if (guideObject.enableRot)
            {
                guideObject.transformTarget.localEulerAngles = Vector3.zero;
                guideObject.changeAmount.rot = guideObject.transformTarget.localEulerAngles;
            }
        }
    }
}
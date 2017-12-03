using System;
using UnityEngine;

namespace FkAssistPlugin.HSStudioNEOAddno
{
  public class ShaderUtil
  {
    private static Shader _TransparentZAlways;
    private static Shader _SpriteDefaultZAlways;
    private static Shader _AlphaVertexlit;

    static ShaderUtil()
    {
      ShaderUtil.LoadShaders();
    }

    private static void LoadShaders()
    {
      ShaderUtil.FindAndSetShader(Resources.FindObjectsOfTypeAll<Shader>());
      if (!((UnityEngine.Object) ShaderUtil._TransparentZAlways == (UnityEngine.Object) null))
        return;
      try
      {
        AssetBundle assetBundle = AssetBundle.LoadFromFile("abdata/studio/object/driving rig.unity3d");
        assetBundle.LoadAllAssets();
        ShaderUtil.FindAndSetShader(Resources.FindObjectsOfTypeAll<Shader>());
        int num = 0;
        assetBundle.Unload(num != 0);
      }
      catch (Exception ex)
      {
        Console.WriteLine((object) ex);
      }
    }

    private static void FindAndSetShader(Shader[] shaders)
    {
      for (int index = 0; index < shaders.Length; ++index)
      {
        Shader shader = shaders[index];
        if (shader.name == "Custom/TransparentZAlways")
          ShaderUtil._TransparentZAlways = shader;
        if (shader.name == "Custom/SpriteDefaultZAlways")
          ShaderUtil._SpriteDefaultZAlways = shader;
        else if (shader.name == "Legacy Shaders/Transparent/VertexLit")
          ShaderUtil._AlphaVertexlit = shader;
        if ((UnityEngine.Object) ShaderUtil._TransparentZAlways != (UnityEngine.Object) null && (UnityEngine.Object) ShaderUtil._SpriteDefaultZAlways != (UnityEngine.Object) null && (UnityEngine.Object) ShaderUtil._AlphaVertexlit != (UnityEngine.Object) null)
          break;
      }
    }

    public static Shader TransparentZAlways
    {
      get
      {
        return ShaderUtil._TransparentZAlways;
      }
    }

    public static Shader AlphaVertexLit
    {
      get
      {
        return ShaderUtil._AlphaVertexlit;
      }
    }

    public static Shader SpriteDefaultZAlways
    {
      get
      {
        return ShaderUtil._SpriteDefaultZAlways;
      }
    }
  }
}
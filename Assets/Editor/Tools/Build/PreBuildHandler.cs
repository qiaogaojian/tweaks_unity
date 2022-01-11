using System.IO;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;

public class PreBuildHandler : IPreprocessBuildWithReport, IPostprocessBuildWithReport, IProcessSceneWithReport
{
    public int callbackOrder
    {
        get => 0;
    }

    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log("Before Build");
        CopyMD2SteamingAssets();
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        Debug.Log("After Build");
    }

    public void OnProcessScene(Scene scene, BuildReport report)
    {
        Debug.Log("OnProcessScene: " + scene.name);
        // 能在场景中添加游戏物体
        // GameObject.CreatePrimitive(scene.buildIndex == 0 ? PrimitiveType.Cube : PrimitiveType.Cylinder);
    }

    /**
     * 所有平台统一使用markdown来显示菜单
     */
    private void CopyMD2SteamingAssets()
    {
        string sourcePath = Application.dataPath + "/../Readme.md";
        string targetPath = Application.dataPath + "/StreamingAssets";
        if (!Directory.Exists(targetPath))
        {
            Directory.CreateDirectory(targetPath);
        }

        File.Copy(sourcePath, targetPath + "/Readme.md", true);
    }
}
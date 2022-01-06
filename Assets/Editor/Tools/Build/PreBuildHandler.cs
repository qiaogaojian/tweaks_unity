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
        Debug.Log("OnPreprocessBuild");
        copyMd();
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        Debug.Log("OnPostprocessBuild");
    }

    public void OnProcessScene(Scene scene, BuildReport report)
    {
        Debug.Log("OnProcessScene: " + scene.name);
        // 能在场景中添加游戏物体
        GameObject.CreatePrimitive(scene.buildIndex == 0 ? PrimitiveType.Cube : PrimitiveType.Cylinder);
    }

    public void copyMd()
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
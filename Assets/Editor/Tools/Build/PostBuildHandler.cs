using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

/// <summary>
/// 导出工程后复制文件到工程里
/// </summary>
public class PostBuildHandler : MonoBehaviour
{
    [PostProcessBuild(1)]
    public static void AfterBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.Android)
        {
            string sourcePath = Application.dataPath + "/../Readme.md";
            string targetPath = GetStreamingAssetsPathInBuild(target, pathToBuiltProject);
            Debug.Log($"target:{target} \tpathToBuildProject:{pathToBuiltProject} \t sourcePath:{sourcePath} \t targetPath:{targetPath}");

            // if (!Directory.Exists(targetPath))
            // {
            //     Directory.CreateDirectory(targetPath);
            // }
            //
            // File.Copy(sourcePath, targetPath + "/Readme.md", true);
        }
    }

    static string GetStreamingAssetsPathInBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target.ToString().Contains("OSX"))
        {
            return pathToBuiltProject + "/Contents/Resources/Data/StreamingAssets/";
        }

        if (target.ToString().Contains("Windows"))
        {
            string name = pathToBuiltProject.Substring(pathToBuiltProject.LastIndexOf('/') + 1).Replace(".exe", "");
            return pathToBuiltProject + "/" + name + "_Data/StreamingAssets/";
        }

        if (target == BuildTarget.Android)
        {
            return pathToBuiltProject + "/unityLibrary/src/main/assets/";
        }

        if (target == BuildTarget.iOS)
        {
            return pathToBuiltProject + "/Data/Raw/";
        }

        if (target == BuildTarget.WebGL)
        {
            return pathToBuiltProject + "/StreamingAssets/";
        }

        throw new UnityException("Platform not implemented");
    }
}
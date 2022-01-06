using UnityEngine;
using System.IO;
using UnityEditor;

namespace Mega
{
    /// <summary>
    /// Csv to game data.CSV静态表读取工具
    /// </summary>
    public class CsvToGameData
    {
        // CSV路径,格式为Assets/StreamingAssets/下的相对目录
        private static string csvPath = "/Editor/CSV/1/";

        // 生成脚本的路径,格式为Resources/下的相对目录
        public static string scriptPath = "/Scripts/Data/Table/";

        [MenuItem("Tools/CSVToGameData")]
        private static void CsvCreatScript()
        {
            CreatScript();
        }

        /// <summary>
        /// 创建脚本
        /// </summary>
        public static void CreatScript()
        {
            string spath = Application.dataPath + scriptPath;
            if (Directory.Exists(spath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(spath);
                FileInfo[]    files         = directoryInfo.GetFiles("*", SearchOption.AllDirectories);

                for (int i = 0; i < files.Length; i++)
                {
                    File.Delete(files[i].FullName);
                }
            }

            if (Directory.Exists(Application.dataPath + csvPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + csvPath);
                FileInfo[]    files         = directoryInfo.GetFiles("*", SearchOption.AllDirectories);

                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Extension.Equals(".csv"))
                    {
                        Debug.Log("解析文件：" + files[i].Name);
                        CreateClass.CreateStruct(CsvLoader.CreatCsvModelScript(files[i].FullName, files[i].Name.Split('.')[0]));
                    }
                }
            }
        }
    }
}
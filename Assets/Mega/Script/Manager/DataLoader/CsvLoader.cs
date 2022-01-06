using UnityEngine;
using System.Collections.Generic;
using System;

namespace Mega
{
    /// <summary>
    /// CSV读取工具
    /// </summary>
    public class CsvLoader
    {
        //public static List<string> names=new List<string> (); //TODO delete

        /// <summary>
        /// 加载csv文件为字符数组
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <returns></returns>
        public static string[] LoadFileLines(string relativePath)
        {
            // byte[] bytes = FileUtils.LoadFileBytes(relativePath, PathMode.Streaming);
            //todo 准备新增aes加密
            string data = FileUtils.LoadFile(relativePath,PathMode.Data);
            return data.Split(new String[] {"\r\n"}, StringSplitOptions.None);
        }


        /// <summary>
        /// 读取CSV文件
        /// 结果保存到字典集合，以ID作为Key值，对应每一行的数据，每一行的数据也用字典集合保存。
        /// </summary>
        /// <param name="filePath">CSV文件路径</param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> LoadCsvFile(string filePath)
        {
            //整张CSV数据表的数据模型
            Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();

            //读入CSV数据 如果数据格式不对就退出
            Debuger.Log("解析文件：" + filePath, Debuger.ColorType.green);
            string[] fileData = LoadFileLines(filePath); //得到以行为单位的数据集合
            //Debug.Log("fileData.Length："+fileData.Length);
            if (fileData.Length < 4) //第一行为说明（不需要读取），CSV文件的第二行为Key字段，第三行开始是数据。第一个字段一定是ID。
            {
                Debuger.LogError("<color=yellow>CSV文件格式填写错误! 数据表少于4行, 数据表格式为: 第一行为说明,第二行为变量名,第三行为变量类型,第四行已经以后为数据</color>");
                return result;
            }
            //得到CSV文件名 作为生成数据模型脚本的名字
            //string[] fileNames=filePath.Split('/');
            //string fileName=fileNames[fileNames.Length-1].Split('.')[0];
            //PlayerPrefs.SetString();

            //给每一行数据模型的变量赋值
            string[] keys = fileData[1].Split(',');
            //string[] types      =   fileData [2].Split (',');
            if (keys.Length < 1)
            {
                Debuger.Log(fileData[1]);
            }

            //把CSV数据装入CSV总模型 从数据表的第4行开始为数据
            for (int i = 3; i < fileData.Length; i++)
            {
                if (string.IsNullOrEmpty(fileData[i]))
                {
                    //Debuger.LogError("数据行为空.");
                    continue;
                }

                //获得每一行的数据
                string[] lineData = fileData[i].Split(',');

                //以行的ID为key值，创建一个新的集合，用于保存当前行的数据
                string rowID = lineData[0];
                result[rowID] = new Dictionary<string, string>();
                for (int columnID = 0; columnID < lineData.Length; columnID++)
                {
                    if (columnID >= keys.Length)
                    {
                        Debuger.LogError("数组越界");
                    }

                    if (columnID >= lineData.Length)
                    {
                        Debuger.LogError("数组越界");
                    }

                    //每一行的数据存储规则：Key字段-Value值
                    result[rowID][keys[columnID]] = lineData[columnID];
                }
            }

            return result;
        }


        /// <summary>
        /// 读取CSV文件
        /// 结果保存到字典集合，以ID作为Key值，对应每一行的数据，每一行的数据也用字典集合保存。
        /// </summary>
        /// <param name="filePath">CSV文件路径</param>
        /// <returns></returns>
        public static DataModel CreatCsvModelScript(string filePath, string name)
        {
            //CSV每一行数据的数据模型
            DataModel dataModel = new DataModel();


            //读入CSV数据 如果数据格式不对就退出
            string[] fileData = LoadFileLines(filePath); //得到以行为单位的数据集合
            if (fileData.Length < 4)                     //第一行为说明（不需要读取），CSV文件的第二行为Key字段，第三行开始是数据。第一个字段一定是ID。
            {
                Debug.Log("文件读取错误,路径：" + filePath);
                Debug.Log("<color=yellow>CSV文件格式填写错误!" + filePath + " 数据表少于4行, 数据表格式为: 第一行为说明,第二行为变量名,第三行为变量类型,第四行已经以后为数据</color>");
                return null;
            }

            dataModel.Name = name;

            //给每一行数据模型的变量赋值
            string[] keys     = fileData[1].Split(',');
            string[] types    = fileData[2].Split(',');
            string[] comments = fileData[0].Split(',');

            foreach (string key in keys)
            {
                dataModel.Key.Add(key);
            }

            foreach (string type in types)
            {
                dataModel.Type.Add(type);
            }

            foreach (string comment in comments)
            {
                dataModel.Comment.Add(comment);
            }

            //把CSV数据装入CSV总模型 从数据表的第4行开始为数据
            for (int i = 3; i < fileData.Length; i++)
            {
                //获得每一行的数据
                string[] lineData = fileData[i].Split(',');
                foreach (var data in lineData)
                {
                    dataModel.Data.Add(data);
                }
            }

            return dataModel;
        }
    }

    /// <summary>
    /// CSV每一行数据的数据模型
    /// </summary>
    public class DataModel
    {
        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private List<string> key = new List<string>();

        public List<string> Key
        {
            get { return key; }
            set { key = value; }
        }

        private List<string> type = new List<string>();

        public List<string> Type
        {
            get { return type; }
            set { type = value; }
        }

        private List<string> comment = new List<string>();

        public List<string> Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        private List<string> data = new List<string>();

        public List<string> Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}
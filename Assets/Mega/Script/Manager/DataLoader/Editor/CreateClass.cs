using UnityEngine;
using System.Text;
using System.IO;

namespace Mega
{
    /// <summary>
    /// 创建数据模型的C#脚本
    /// </summary>
    public class CreateClass
    {
        /// <summary>
        /// 创建CSV数据模型的C#脚本
        /// </summary>
        /// <param name="dataModel">数据模型</param>
        public static void CreateStruct(DataModel dataModel)
        {
            StringBuilder codeDic = new StringBuilder();

            #region 构造行数据模型

            codeDic.Append("using UnityEngine;\n");
            codeDic.Append("using System.Collections;\n");
            codeDic.Append("using System.Collections.Generic;\n\n");

            codeDic.Append("[System.Serializable]\n");
            codeDic.Append("public class " + dataModel.Name + "\n{\n");

            for (int i = 0; i < dataModel.Key.Count; i++)
            {
                codeDic.Append("\t//" + dataModel.Comment[i] + " \n");
                codeDic.Append("\tpublic " + dataModel.Type[i] + ' ' + dataModel.Key[i] + "{ get;set; }\n");
            }

            codeDic.Append("\n\tpublic " + dataModel.Name + "(" + dataModel.Name + " data" + ") \n\t{\n");
            for (int i = 0; i < dataModel.Key.Count; i++)
            {
                codeDic.Append("\t\t" + dataModel.Key[i] + "= data." + dataModel.Key[i] + ";\n");
            }

            codeDic.Append("\t}\n");

            codeDic.Append("\n\tpublic " + dataModel.Name + "( ) \n\t{\n");

            codeDic.Append("\t}\n");

            codeDic.Append("}\n");
            codeDic.Append("\n");

            #endregion

            #region 构造行列数据模型

            codeDic.Append("public class " + dataModel.Name + "Table:ScriptableObject" + " \n{\n");
            codeDic.Append("\tpublic List<" + dataModel.Name + ">" + dataModel.Name + "List = new List<" + dataModel.Name + "> ();\n}");

            #endregion

            string outStr = codeDic.ToString();

            #region 处理特殊的数据结构

            if (outStr.Contains("int[:]"))
            {
                Debuger.Log("int[:]替换为List<List<int>>");
                outStr = outStr.Replace("int[:]", "List<List<int>>");
            }

            #endregion

            #region 把构造的脚本写到文件

            string CSharpFilePath = Application.dataPath + CsvToGameData.scriptPath;

            FileStream   fs = new FileStream(CSharpFilePath + "/" + dataModel.Name + ".cs", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, new UTF8Encoding(true));

            sw.Write(outStr);
            sw.Close();
            fs.Close();
            Debuger.Log("写入静态数据表:" + dataModel.Name);

            #endregion
        }
    }
}
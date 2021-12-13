//**********************************************************************
//# by Michael
//# at 12/3/2017 8:48:52 PM
//**********************************************************************
/*
*功能：测试Json.NET
*/

using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using Mega;

public class TestJsonConvert : MonoBehaviour
{
    public const string                  ConstValue = "1";
    private      TestJsonObject          jsonObject;
    private      TestJsonArray           testJsonArray;
    private      TestJsonDictionary      testJsonDictionary;
    private      List<TestJsonObject>    jsonArray      = new List<TestJsonObject>();
    private      Dictionary<int, string> jsonDictionary = new Dictionary<int, string>();
    private      string                  jsonFilePath;
    private      string                  jsonFileName;

    void Start()
    {
        jsonObject = new TestJsonObject(1001, "Michael");
        jsonArray.Add(jsonObject);
        jsonDictionary.Add(jsonObject.ID, jsonObject.Name);

        jsonObject = new TestJsonObject(1002, "Michelle");
        jsonArray.Add(jsonObject);
        jsonDictionary.Add(jsonObject.ID, jsonObject.Name);

        InitJson();
        jsonFilePath = Application.dataPath + "/ZTest/JsonData.json";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //序列化
            string json = JsonConvert.SerializeObject(jsonObject);
            Debuger.Log("序列化对象 JsonObject:" + json);

            //反序列化
            TestJsonObject obj = JsonConvert.DeserializeObject<TestJsonObject>(json);
            obj.ID   = 100;
            obj.Name = "Smith";
            Debuger.Log("反序列化对象 JsonObject:" + JsonConvert.SerializeObject(obj));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //序列化
            string json = JsonConvert.SerializeObject(jsonArray);
            Debuger.Log("序列化数组 JsonArray:" + json);

            //反序列化
            List<TestJsonObject> jarray = JsonConvert.DeserializeObject<List<TestJsonObject>>(json);
            Debuger.Log("反序列化数组1 JsonArray:" + JsonConvert.SerializeObject(jarray));

            JArray array = JArray.Parse(json);
            for (int i = 1; i <= array.Count; i++)
            {
                TestJsonObject obj = new TestJsonObject();
                obj      = JsonConvert.DeserializeObject<TestJsonObject>(array[i - 1].ToString());
                obj.ID   = 1002 + i;
                obj.Name = "NO." + i;
                jsonArray.Add(obj);
            }

            Debuger.Log("反序列化数组2 JsonArray:" + JsonConvert.SerializeObject(jsonArray));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //序列化
            string json = JsonConvert.SerializeObject(jsonDictionary);
            Debuger.Log("序列化字典 JsonDictionary:" + json);

            //反序列化
            Dictionary<int, string> jsonDic = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);
            jsonDic.Add(1007, "James");
            Debuger.Log("反序列化字典 JsonDictionary:" + JsonConvert.SerializeObject(jsonDic));
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SerializeObject();

            DeserializeJson();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SerializeObjectToFile<List<TestJsonObject>>(jsonFilePath, jsonArray);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            DeserializeJsonFromFile<List<TestJsonObject>>(jsonFilePath);
        }
    }

    #region 序列化反序列化示例

    string jsonData;
    Table1 table1;
    Table2 table2;
    Table3 table3;

    void InitJson()
    {
        jsonData = @"{
                      ""sites"": {
                        ""site"": 
                        [
                          {
                            ""id""  : ""1001"",
                            ""name"": ""Mega"",
                            ""url"" : ""MegaGameFramework""
                          },
                          {
                            ""id""  : ""1002"",
                            ""name"": ""Michael"",
                            ""url"" : ""I am Best!""
                          },
                          {
                            ""id""  : ""1003"",
                            ""name"": ""Google"",
                            ""url"" : ""www.google.com""
                          }
                        ]
                      }
                    }";
    }

    void DeserializeJson()
    {
        table1 = JsonConvert.DeserializeObject<Table1>(jsonData); //反序列化对象

        Debuger.Log(table1.sites.site[0].id);
        Debuger.Log(table1.sites.site[0].name);
        Debuger.Log(table1.sites.site[0].url);
    }

    void SerializeObject()
    {
        Student student = new Student();
        student.name   = "Michael";
        student.age    = 20;
        student.gender = "Man";

        string json = JsonConvert.SerializeObject(student, Formatting.Indented); //序列化对象
        Debuger.Log(json);
    }

    #endregion

    #region 序列化对象到本地 从本地反序列化对象

    void DeserializeJsonFromFile<T>(string path)
    {
        string json = File.ReadAllText(path);
        T      data = JsonConvert.DeserializeObject<T>(json);
        Debuger.Log("从本地文件反序列Json成功 " + JsonConvert.SerializeObject(data));
    }

    void SerializeObjectToFile<T>(string path, T obj)
    {
        string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
        File.WriteAllText(path, json);
        Debuger.Log("写入Json文件成功");
    }

    #endregion
}

#region 用来反序列化Json的模板类

public class TestJsonObject
{
    public int ID { get; set; }
    public string Name { get; set; }

    public TestJsonObject()
    {
    }

    public TestJsonObject(int id, string name)
    {
        this.ID   = id;
        this.Name = name;
    }
}

public class TestJsonArray
{
    public List<TestJsonObject> JsonArray = new List<TestJsonObject>();

    public TestJsonArray(List<TestJsonObject> jsonArray)
    {
        JsonArray = jsonArray;
    }
}

public class TestJsonDictionary
{
    public Dictionary<int, string> JsonDictionary = new Dictionary<int, string>();

    public TestJsonDictionary(Dictionary<int, string> jsonDictionary)
    {
    }
}

public class Table1
{
    public Table2 sites { get; set; }
}

public class Table2
{
    public List<Table3> site { get; set; }
}

public class Table3
{
    public int id { get; set; }
    public string name { get; set; }
    public string url { get; set; }
}

/// <summary>
/// 用来序列化对象的模板类
/// </summary>
public class Student
{
    public string name { get; set; }
    public int age { get; set; }
    public string gender { get; set; }
}

#endregion
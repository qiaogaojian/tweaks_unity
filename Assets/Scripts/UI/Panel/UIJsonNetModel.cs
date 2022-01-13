using System;
using System.Collections.Generic;
using Mega;
using UnityEngine;

namespace Game
{
    public class UIJsonNetModel : BaseViewModel
    {
        public TestJsonObject          jsonObject;
        public Student                 student;
        public List<TestJsonObject>    jsonArray      = new List<TestJsonObject>();
        public Dictionary<int, string> jsonDictionary = new Dictionary<int, string>();


        public Table1 table1;
        public Table2 table2;
        public Table3 table3;

        public string jsonFilePath = Application.persistentDataPath + "/JsonData.json";

        public string jsonData = @"
        {
          ""sites"": {
            ""site"": [
              {
                ""id"": ""1001"",
                ""name"": ""Google"",
                ""url"": ""www.google.com""
              },
              {
                ""id"": ""1002"",
                ""name"": ""Bing"",
                ""url"": ""www.bing.com""
              },
              {
                ""id"": ""1003"",
                ""name"": ""Sogou"",
                ""url"": ""www.sogou.com""
              }
            ]
          }
        }";

        public override void Init(Action onFinish = null)
        {
            initData();
            onFinish?.Invoke();
        }

        private void initData()
        {
            jsonObject = new TestJsonObject(1001, "Michael");
            jsonArray.Add(jsonObject);
            jsonDictionary.Add(jsonObject.ID, jsonObject.Name);

            jsonObject = new TestJsonObject(1002, "Michelle");
            jsonArray.Add(jsonObject);
            jsonDictionary.Add(jsonObject.ID, jsonObject.Name);

            student        = new Student();
            student.name   = "Michael";
            student.age    = 20;
            student.gender = "Man";
        }

        public override void Destroy()
        {
        }
    }
}
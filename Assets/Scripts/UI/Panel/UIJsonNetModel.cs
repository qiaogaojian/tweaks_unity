using System;
using System.Collections.Generic;
using Mega;
using UnityEngine;

namespace Game
{
    public class UIJsonNetModel : BaseViewModel
    {
        public TestJsonObject          jsonObject;
        public TestJsonObjects         jsonObjects;
        public Student                 student;
        public List<TestJsonObject>    jsonArray      = new List<TestJsonObject>();
        public Dictionary<int, string> jsonDictionary = new Dictionary<int, string>();


        public Table1 table1;
        public Table2 table2;
        public Table3 table3;

        public string jsonArrayPath = Application.persistentDataPath + "/JsonArray.json";
        public string jsonObjectPath = Application.persistentDataPath + "/JsonObject.json";

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
            for (int i = 0; i < 100000; i++)
            {
                jsonObject = new TestJsonObject(i, $"item{i}");
                jsonArray.Add(jsonObject);
                jsonDictionary.Add(jsonObject.ID, jsonObject.Name);
            }

            jsonObjects           = new TestJsonObjects();
            jsonObjects.jsonArray = jsonArray;

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
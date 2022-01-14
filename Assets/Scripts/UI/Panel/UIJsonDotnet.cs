using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Game;
using Mega;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIJsonDotnet : BaseView
{
    private Button          btnReturn;
    private Button          btnSerialize1;
    private Button          btnSerialize2;
    private Button          btnSerialize3;
    private Button          btnSerialize4;
    private Button          btnSerialize5;
    private Button          btnSerialize6;
    private Button          btnDeserialize1;
    private Button          btnDeserialize2;
    private Button          btnDeserialize3;
    private Button          btnDeserialize4;
    private Button          btnDeserialize5;
    private Button          btnDeserialize6;
    private TextMeshProUGUI tvTime;

    private UIJsonNetModel dataModel;

    public override void InitView()
    {
        dataModel = new UIJsonNetModel();
        dataModel.Init();

        btnReturn       = transform.Find("btnReturn").GetComponent<Button>();
        btnSerialize1   = transform.Find("ivBg/btnSerialize1").GetComponent<Button>();
        btnSerialize2   = transform.Find("ivBg/btnSerialize2").GetComponent<Button>();
        btnSerialize3   = transform.Find("ivBg/btnSerialize3").GetComponent<Button>();
        btnSerialize4   = transform.Find("ivBg/btnSerialize4").GetComponent<Button>();
        btnSerialize5   = transform.Find("ivBg/btnSerialize5").GetComponent<Button>();
        btnSerialize6   = transform.Find("ivBg/btnSerialize6").GetComponent<Button>();
        btnDeserialize1 = transform.Find("ivBg/btnDeserialize1").GetComponent<Button>();
        btnDeserialize2 = transform.Find("ivBg/btnDeserialize2").GetComponent<Button>();
        btnDeserialize3 = transform.Find("ivBg/btnDeserialize3").GetComponent<Button>();
        btnDeserialize4 = transform.Find("ivBg/btnDeserialize4").GetComponent<Button>();
        btnDeserialize5 = transform.Find("ivBg/btnDeserialize5").GetComponent<Button>();
        btnDeserialize6 = transform.Find("ivBg/btnDeserialize6").GetComponent<Button>();
        tvTime          = transform.Find("ivBg/tvTime").GetComponent<TextMeshProUGUI>();
    }


    protected override void AddEvent()
    {
        btnReturn.onClick.AddListener(OnClickBtnReturn);

        btnSerialize1.onClick.AddListener(OnClickBtnSerializeObjet);
        btnSerialize2.onClick.AddListener(OnClickBtnSerializeArray);
        btnSerialize3.onClick.AddListener(OnClickBtnSerializeMap);
        btnSerialize4.onClick.AddListener(OnClickBtnSerializeObject2);
        btnSerialize5.onClick.AddListener(OnClickBtnSerializeToFile);
        btnSerialize6.onClick.AddListener(OnClickBtnSerializeToFileOptimize);

        btnDeserialize1.onClick.AddListener(OnClickBtnDeSerializeObjet);
        btnDeserialize2.onClick.AddListener(OnClickBtnDeSerializeArray);
        btnDeserialize3.onClick.AddListener(OnClickBtnDeSerializeMap);
        btnDeserialize4.onClick.AddListener(OnClickBtnDeSerializeObject2);
        btnDeserialize5.onClick.AddListener(OnClickBtnDeSerializeFromFile);
        btnDeserialize6.onClick.AddListener(OnClickBtnDeSerializeObjetFileOptimize);
    }

    protected override void RemoveEvent()
    {
        btnReturn.onClick.RemoveListener(OnClickBtnReturn);

        btnSerialize1.onClick.RemoveListener(OnClickBtnSerializeObjet);
        btnSerialize2.onClick.RemoveListener(OnClickBtnSerializeArray);
        btnSerialize3.onClick.RemoveListener(OnClickBtnSerializeMap);
        btnSerialize4.onClick.RemoveListener(OnClickBtnSerializeObject2);
        btnSerialize5.onClick.RemoveListener(OnClickBtnSerializeToFile);
        btnSerialize6.onClick.RemoveListener(OnClickBtnSerializeToFileOptimize);

        btnDeserialize1.onClick.RemoveListener(OnClickBtnDeSerializeObjet);
        btnDeserialize2.onClick.RemoveListener(OnClickBtnDeSerializeArray);
        btnDeserialize3.onClick.RemoveListener(OnClickBtnDeSerializeMap);
        btnDeserialize4.onClick.RemoveListener(OnClickBtnDeSerializeObject2);
        btnDeserialize5.onClick.RemoveListener(OnClickBtnDeSerializeFromFile);
        btnDeserialize6.onClick.RemoveListener(OnClickBtnDeSerializeObjetFileOptimize);
    }

    private void OnClickBtnReturn()
    {
        Framework.UI.HideCurrent();
    }

    private void Update()
    {
        tvTime.text = Time.time.ToString("0.000");
    }


    #region 序列化

    /// <summary>
    /// 序列化对象
    /// </summary>
    private void OnClickBtnSerializeObjet()
    {
        //序列化
        string json = JsonConvert.SerializeObject(dataModel.jsonObject);
        Debuger.Log("序列化对象 JsonObject:" + json);
    }

    /// <summary>
    /// 序列化数组
    /// </summary>
    private void OnClickBtnSerializeArray()
    {
        string json = JsonConvert.SerializeObject(dataModel.jsonArray);
        Debuger.Log("序列化数组 JsonArray:" + json);
    }

    /// <summary>
    /// 序列化字典
    /// </summary>
    private void OnClickBtnSerializeMap()
    {
        string json = JsonConvert.SerializeObject(dataModel.jsonDictionary);
        Debuger.Log("序列化字典 JsonDictionary:" + json);
    }

    /// <summary>
    /// 序列化对象
    /// </summary>
    private void OnClickBtnSerializeObject2()
    {
        string json = JsonConvert.SerializeObject(dataModel.student, Formatting.Indented); //格式化输出
        Debuger.Log("序列化对象2 JsonObject:" + json);
    }

    /// <summary>
    /// 序列化数组到文件中
    /// </summary>
    private void OnClickBtnSerializeToFile()
    {
        long timeB = Tools.GetTimeStamp();
        SerializeObjectToFile(dataModel.jsonFilePath, dataModel.jsonArray);
        long     timeA = Tools.GetTimeStamp();
        DateTime time  = Tools.LongDateTimeToDateTimeString(timeA - timeB);
        Debuger.Log($"SerializeObjetFile 花费时间:{time.Second}秒");
    }

    /// <summary>
    /// 序列化数组到文件中
    /// </summary>
    private void OnClickBtnSerializeToFileOptimize()
    {
        SerializeObjectToFile(dataModel.jsonFilePath, dataModel.jsonArray);
    }

    /// <summary>
    /// 序列化对象到json文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    private void SerializeObjectToFile<T>(string path, T obj)
    {
        using (FileStream fs = new FileStream(path, FileMode.CreateNew))
        using (StreamWriter sw = new StreamWriter(fs))
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            sw.Write(json);
        }

        Debuger.Log("写入Json文件成功");
    }

    #endregion


    #region 反序列化

    /// <summary>
    /// 反序列化对象
    /// </summary>
    private void OnClickBtnDeSerializeObjet()
    {
        //序列化
        string json = JsonConvert.SerializeObject(dataModel.jsonObject);

        //反序列化
        TestJsonObject obj = JsonConvert.DeserializeObject<TestJsonObject>(json);
        obj.ID   = 100;
        obj.Name = "Smith";
        Debuger.Log("反序列化对象 JsonObject:" + JsonConvert.SerializeObject(obj));
    }

    /// <summary>
    /// 反序列化数组
    /// </summary>
    private void OnClickBtnDeSerializeArray()
    {
        //序列化
        string json = JsonConvert.SerializeObject(dataModel.jsonArray);

        //反序列化数组方法1
        List<TestJsonObject> jarray = JsonConvert.DeserializeObject<List<TestJsonObject>>(json);
        Debuger.Log("反序列化数组方法1 JsonArray:" + JsonConvert.SerializeObject(jarray));

        // 反序列化数组方法2
        JArray               array     = JArray.Parse(json);
        List<TestJsonObject> jsonArray = new List<TestJsonObject>();
        for (int i = 1; i <= array.Count; i++)
        {
            TestJsonObject obj = new TestJsonObject();
            obj      = JsonConvert.DeserializeObject<TestJsonObject>(array[i - 1].ToString());
            obj.ID   = i;
            obj.Name = "NO." + i;
            jsonArray.Add(obj);
        }

        Debuger.Log("反序列化数组方法2 JsonArray:" + JsonConvert.SerializeObject(jsonArray));
    }

    /// <summary>
    /// 反序列化字典
    /// </summary>
    private void OnClickBtnDeSerializeMap()
    {
        //序列化
        string json = JsonConvert.SerializeObject(dataModel.jsonDictionary);

        //反序列化
        Dictionary<int, string> jsonDic = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);
        jsonDic.Add(1003, "James");
        Debuger.Log("反序列化字典 JsonDictionary:" + JsonConvert.SerializeObject(jsonDic));
    }

    /// <summary>
    /// 反序列化对象2
    /// </summary>
    private void OnClickBtnDeSerializeObject2()
    {
        dataModel.table1 = JsonConvert.DeserializeObject<Table1>(dataModel.jsonData); //反序列化对象
        Debuger.Log("反序列化对象 JsonObject:" + JsonConvert.SerializeObject(dataModel.table1));
    }

    /// <summary>
    /// 从本地json文件中反序列化对象
    /// </summary>
    private void OnClickBtnDeSerializeFromFile()
    {
        long timeB = Tools.GetTimeStamp();
        DeserializeJsonArrayFromFile<TestJsonObject>(dataModel.jsonFilePath);
        long     timeA = Tools.GetTimeStamp();
        DateTime time  = Tools.LongDateTimeToDateTimeString(timeA - timeB);
        Debuger.Log($"DeSerializeObjetFile 花费时间:{time.Second}秒");
    }

    /// <summary>
    /// 从本地json文件中反序列化对象
    /// </summary>
    private void OnClickBtnDeSerializeObjetFileOptimize()
    {
        StartCoroutine(DeserializeJsonArrayFromFileAsync<TestJsonObject>(dataModel.jsonFilePath));
    }

    /// <summary>
    /// 从本地json文件中反序列化对象
    /// </summary>
    /// <param name="path"></param>
    /// <typeparam name="T"></typeparam>
    private void DeserializeJsonArrayFromFile<T>(string path)
    {
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        using (StreamReader sr = new StreamReader(fs))
        using (JsonReader reader = new JsonTextReader(sr))
        {
            JsonSerializer serializer = new JsonSerializer();
            List<T>        data       = serializer.Deserialize<List<T>>(reader);
            Debuger.Log($"从本地文件反序列Json成功 size:{data.Count}");
        }
    }

    private IEnumerator DeserializeJsonArrayFromFileAsync<K>(string path)
    {
        long timeB = Tools.GetTimeStamp();

        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        using (StreamReader sr = new StreamReader(fs))
        {
            List<K> datas = new List<K>();
            int     count = 0;
            foreach (var data in Tools.DeserializeValues<K>(sr))
            {
                count++;
                if (count >= 10000) // 每帧处理10000条数据 防止卡死
                {
                    count = 0;
                    yield return data;
                }

                datas.Add(data);
            }

            Debuger.Log($"从本地文件反序列Json成功 size:{datas.Count}");
        }

        long     timeA = Tools.GetTimeStamp();
        DateTime time  = Tools.LongDateTimeToDateTimeString(timeA - timeB);
        Debuger.Log($"DeSerializeObjetFile 花费时间:{time.Second}秒");
    }

    #endregion
}
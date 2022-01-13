//**********************************************************************
//# by Michael
//# at 12/3/2017 7:48:10 PM
//**********************************************************************
/*
*功能：测试事件管理器
*/

using Newtonsoft.Json;
using UnityEngine;


public class TestEventManager : MonoBehaviour
{
    private TestEventData eventData;

    void Awake()
    {
        RegisterEvent();

        eventData = new TestEventData("Michael", "Man", 18);
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     EventManager.Instance.SendEvent(EventId.TEST_EventNormal);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     EventManager.Instance.SendEvent(EventId.TEST_EventParamInt, 666);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.Alpha3))
        // {
        //     EventManager.Instance.SendEvent(EventId.TEST_EventParamObject, eventData);
        // }
    }

    //无参回调
    void TEST_EventNormal()
    {
        Debuger.Log("TEST_EventNormal");
    }

    //Int参数回调
    void TEST_EventParamInt(int param)
    {
        int data = param;
        Debuger.Log("TEST_EventParamInt ParamData:" + data);
    }

    //对象参数回调
    void TEST_EventParamObject(TestEventData param)
    {
        TestEventData eventData = param;
        string        json      = JsonConvert.SerializeObject(eventData);
        Debuger.Log("TEST_EventParamObject ParamData:" + json);
    }

    void RegisterEvent()
    {
        // Framework.Event.AddEventListener(EventId.TEST_EventNormal, TEST_EventNormal);
        // Framework.Event.AddEventListener<int>(EventId.TEST_EventParamInt, TEST_EventParamInt);
        // Framework.Event.AddEventListener<TestEventData>(EventId.TEST_EventParamObject, TEST_EventParamObject);
    }

    void UnRegisterEvent()
    {
        // Framework.Event.RemoveEventListener(EventId.TEST_EventNormal, TEST_EventNormal);
        // Framework.Event.RemoveEventListener<int>(EventId.TEST_EventParamInt, TEST_EventParamInt);
        // Framework.Event.RemoveEventListener<TestEventData>(EventId.TEST_EventParamObject, TEST_EventParamObject);
    }

    void OnDestroy()
    {
        UnRegisterEvent();
    }
}


public class TestEventData
{
    public string Name { get; set; }
    public string Gender { get; set; }
    public int Age { get; set; }

    public TestEventData(string name, string gender, int age)
    {
        this.Name   = name;
        this.Gender = gender;
        this.Age    = age;
    }
}
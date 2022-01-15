using Mega;
using Newtonsoft.Json;
using UnityEngine.UI;

namespace Game
{
    public class UIEvent : BaseView
    {
        private Button btnReturn;
        private Button btnEventNormal;
        private Button btnEventParamInt;
        private Button btnEventParamObject;

        private TestEventData eventData;

        public override void InitView()
        {
            eventData = new TestEventData("Michael", "Man", 18);

            btnReturn           = transform.Find("btnReturn").GetComponent<Button>();
            btnEventNormal      = transform.Find("ivBg/btnEventNormal").GetComponent<Button>();
            btnEventParamInt    = transform.Find("ivBg/btnEventParamInt").GetComponent<Button>();
            btnEventParamObject = transform.Find("ivBg/btnEventParamObject").GetComponent<Button>();
        }

        protected override void AddEvent()
        {
            btnReturn.onClick.AddListener(OnClickBtnReturn);
            btnEventNormal.onClick.AddListener(OnClickBtnEventNormal);
            btnEventParamInt.onClick.AddListener(OnClickBtnEventParamInt);
            btnEventParamObject.onClick.AddListener(OnClickBtnEventParamObject);

            Framework.Event.AddEventListener(EventId.TEST_EventNormal, EventNormal);
            Framework.Event.AddEventListener<int>(EventId.TEST_EventParamInt, EventParamInt);
            Framework.Event.AddEventListener<TestEventData>(EventId.TEST_EventParamObject, EventParamObject);
        }

        protected override void RemoveEvent()
        {
            btnReturn.onClick.RemoveListener(OnClickBtnReturn);
            btnEventNormal.onClick.RemoveListener(OnClickBtnEventNormal);
            btnEventParamInt.onClick.RemoveListener(OnClickBtnEventParamInt);
            btnEventParamObject.onClick.RemoveListener(OnClickBtnEventParamObject);

            Framework.Event.RemoveEventListener(EventId.TEST_EventNormal, EventNormal);
            Framework.Event.RemoveEventListener<int>(EventId.TEST_EventParamInt, EventParamInt);
            Framework.Event.RemoveEventListener<TestEventData>(EventId.TEST_EventParamObject, EventParamObject);
        }


        private void OnClickBtnEventNormal()
        {
            Framework.Event.SendEvent(EventId.TEST_EventNormal);
        }

        private void OnClickBtnEventParamInt()
        {
            Framework.Event.SendEvent(EventId.TEST_EventParamInt, 666);
        }

        private void OnClickBtnEventParamObject()
        {
            Framework.Event.SendEvent(EventId.TEST_EventParamObject, eventData);
        }

        private void OnClickBtnReturn()
        {
            Framework.UI.HideCurrent();
        }

        //无参回调
        void EventNormal()
        {
            Framework.UI.Show<Toast>().MakeText("触发事件: EventNormal");
        }

        //Int参数回调
        void EventParamInt(int param)
        {
            Framework.UI.Show<Toast>().MakeText("触发事件: EventParamInt ParamData:" + param);
        }

        //对象参数回调
        void EventParamObject(TestEventData param)
        {
            string json = JsonConvert.SerializeObject(param);
            Framework.UI.Show<Toast>().MakeText("触发事件: EventParamObject ParamData:" + json);
        }
    }
}
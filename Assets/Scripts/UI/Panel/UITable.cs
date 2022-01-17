using System.Collections.Generic;
using Mega;
using Mega;
using Newtonsoft.Json;
using UnityEngine.UI;

namespace Game
{
    public class UITable : BaseView
    {
        private Button btnReturn;
        private Button btnGetData;
        private Button btnGetTable;

        public override void InitView()
        {
            btnReturn   = transform.Find("btnReturn").GetComponent<Button>();
            btnGetData  = transform.Find("ivBg/btnGetData").GetComponent<Button>();
            btnGetTable = transform.Find("ivBg/btnGetTable").GetComponent<Button>();
        }

        protected override void AddEvent()
        {
            btnReturn.onClick.AddListener(OnClickBtnReturn);
            btnGetData.onClick.AddListener(OnClickBtnbtnGetData);
            btnGetTable.onClick.AddListener(OnClickBtnbtnGetTable);
        }

        protected override void RemoveEvent()
        {
            btnReturn.onClick.RemoveListener(OnClickBtnReturn);
            btnGetData.onClick.RemoveListener(OnClickBtnbtnGetData);
            btnGetTable.onClick.RemoveListener(OnClickBtnbtnGetTable);
        }

        private void OnClickBtnReturn()
        {
            Framework.UI.HideCurrent();
        }

        private void OnClickBtnbtnGetData()
        {
            Localization dataL18N = Framework.Table.GetData<Localization>(0);
            Debuger.Log($"GetData: {JsonConvert.SerializeObject(dataL18N)}");
            Framework.UI.Show<Toast>().MakeText("GetData");
        }

        private void OnClickBtnbtnGetTable()
        {
            Dictionary<int, Localization> tableL18N = Framework.Table.GetTable<Localization>();
            Debuger.Log($"GetTable: {JsonConvert.SerializeObject(tableL18N)}");
            Framework.UI.Show<Toast>().MakeText("GetTable");
        }
    }
}
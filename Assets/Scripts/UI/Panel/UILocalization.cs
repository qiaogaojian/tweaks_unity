using System.Collections.Generic;
using Mega;
using UnityEngine.UI;

namespace Game
{
    public class UILocalization : BaseView
    {
        private Button   btnReturn;
        private Button   btnChinese;
        private Button   btnEnglish;
        private Button   btnJapanese;
        private Dropdown seleLanguage;

        private UILocalizationModel dataModel;

        public override void InitView()
        {
            btnReturn    = transform.Find("btnReturn").GetComponent<Button>();
            btnChinese   = transform.Find("ivBg/btnChinese").GetComponent<Button>();
            btnEnglish   = transform.Find("ivBg/btnEnglish").GetComponent<Button>();
            btnJapanese  = transform.Find("ivBg/btnJapanese").GetComponent<Button>();
            seleLanguage = transform.Find("ivBg/seleLanguage").GetComponent<Dropdown>();

            dataModel = new UILocalizationModel();
            dataModel.Init(() =>
            {
                SetDropDownItemValue(0);
            });
        }

        private void initDropDown(List<Localization> languageData)
        {
            List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();
            for (int i = 0; i < languageData.Count; i++)
            {
                Dropdown.OptionData data = new Dropdown.OptionData();
                data.text = languageData[i].KEY;
            }
        }

        protected override void AddEvent()
        {
            btnReturn.onClick.AddListener(OnClickBtnReturn);
            btnChinese.onClick.AddListener(OnClickBtnChinese);
            btnEnglish.onClick.AddListener(OnClickBtnEnglish);
            btnJapanese.onClick.AddListener(OnClickBtnJapanese);
            seleLanguage.onValueChanged.AddListener(OnSelectLanguage);
        }

        protected override void RemoveEvent()
        {
            btnReturn.onClick.RemoveListener(OnClickBtnReturn);
            btnChinese.onClick.RemoveListener(OnClickBtnChinese);
            btnEnglish.onClick.RemoveListener(OnClickBtnEnglish);
            btnJapanese.onClick.RemoveListener(OnClickBtnJapanese);
            seleLanguage.onValueChanged.RemoveListener(OnSelectLanguage);
        }

        private void OnClickBtnReturn()
        {
            Framework.UI.HideCurrent();
        }

        private void OnClickBtnChinese()
        {
        }

        private void OnClickBtnEnglish()
        {
        }

        private void OnClickBtnJapanese()
        {
        }

        private void OnSelectLanguage(int index)
        {
            Debuger.Log($"Language Index {index}");
        }


        /// <summary>
        /// 添加一个列表下拉数据
        /// </summary>
        /// <param name="listOptions"></param>
        void AddDropDownOptionsData(List<Dropdown.OptionData> listOptions)
        {
            seleLanguage.AddOptions(listOptions);
        }

        /// <summary>
        /// 添加一个下拉数据
        /// </summary>
        /// <param name="itemText"></param>
        void AddDropDownOptionsData(string itemText)
        {
            //添加一个下拉选项
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = itemText;
            //data.image = "指定一个图片做背景不指定则使用默认"；
            seleLanguage.options.Add(data);
        }

        /// <summary>
        /// 设置选择的下拉Item
        /// </summary>
        /// <param name="ItemIndex"></param>
        void SetDropDownItemValue(int ItemIndex)
        {
            if (seleLanguage.options == null)
            {
                Debuger.Log(GetType() + "/SetDropDownItemValue()/下拉列表为空，请检查");
                return;
            }

            if (ItemIndex >= seleLanguage.options.Count)
            {
                ItemIndex = seleLanguage.options.Count - 1;
            }

            if (ItemIndex < 0)
            {
                ItemIndex = 0;
            }

            seleLanguage.value = ItemIndex;
        }
    }
}
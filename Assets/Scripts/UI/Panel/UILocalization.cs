using System;
using System.Collections.Generic;
using Mega;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class UILocalization : BaseView
    {
        private Button btnReturn;
        private Button btnChinese;
        private Button btnEnglish;
        private Button btnJapanese;

        private TMP_DropdownEx      ddlLanguage;
        private TextMeshProUGUI     ddlLabel;
        private UILocalizationModel viewModel;

        public override void InitView()
        {
            btnReturn   = transform.Find("btnReturn").GetComponent<Button>();
            btnChinese  = transform.Find("ivBg/btnChinese").GetComponent<Button>();
            btnEnglish  = transform.Find("ivBg/btnEnglish").GetComponent<Button>();
            btnJapanese = transform.Find("ivBg/btnJapanese").GetComponent<Button>();
            ddlLanguage = transform.Find("ivBg/ddlLanguage").GetComponent<TMP_DropdownEx>();
            ddlLabel    = transform.Find("ivBg/ddlLanguage/Label").GetComponent<TextMeshProUGUI>();


            viewModel = new UILocalizationModel();
            viewModel.Init(() =>
            {
                ddlLanguage.options.Clear();
                AddDropDownOptionsData(viewModel.GetLanguageDropdownData());
                SetDropDownItemValue(0);
            });
        }

        private void initDropDown(List<Localization> languageData)
        {
            List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();
            for (int i = 0; i < languageData.Count; i++)
            {
                TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
                data.text = languageData[i].KEY;
            }
        }

        protected override void AddEvent()
        {
            btnReturn.onClick.AddListener(OnClickBtnReturn);
            btnChinese.onClick.AddListener(OnClickBtnChinese);
            btnEnglish.onClick.AddListener(OnClickBtnEnglish);
            btnJapanese.onClick.AddListener(OnClickBtnJapanese);
            ddlLanguage.onValueChanged.AddListener(OnSelectLanguage);
            ddlLanguage.SetOnClick(() =>
            {
                RefreshLocalizaUI();
            });
        }

        protected override void RemoveEvent()
        {
            btnReturn.onClick.RemoveListener(OnClickBtnReturn);
            btnChinese.onClick.RemoveListener(OnClickBtnChinese);
            btnEnglish.onClick.RemoveListener(OnClickBtnEnglish);
            btnJapanese.onClick.RemoveListener(OnClickBtnJapanese);
            ddlLanguage.onValueChanged.RemoveListener(OnSelectLanguage);
        }

        private void OnClickBtnReturn()
        {
            Framework.UI.HideCurrent();
        }

        private void OnClickBtnChinese()
        {
            Framework.L18N.ChangeLanguage(SystemLanguage.Chinese);
            SetDropDownItemValue(0);
        }

        private void OnClickBtnEnglish()
        {
            Framework.L18N.ChangeLanguage(SystemLanguage.English);
            SetDropDownItemValue(1);
        }

        private void OnClickBtnJapanese()
        {
            Framework.L18N.ChangeLanguage(SystemLanguage.Japanese);
            SetDropDownItemValue(2);
        }

        private void OnSelectLanguage(int index)
        {
            LocalizeData lang = viewModel.GetLanguage(index);
            Debuger.Log($"Language {lang.langText}");
            switch (lang.lang)
            {
                case SystemLanguage.Chinese:
                    Framework.L18N.ChangeLanguage(SystemLanguage.Chinese);
                    break;
                case SystemLanguage.English:
                    Framework.L18N.ChangeLanguage(SystemLanguage.English);
                    break;
                case SystemLanguage.Japanese:
                    Framework.L18N.ChangeLanguage(SystemLanguage.Japanese);
                    break;
            }

            TMP_FontAsset fontAsset = Framework.L18N.GetLangFontAsset(lang.langText);
            ddlLabel.font = fontAsset;
        }


        /// <summary>
        /// 添加一个列表下拉数据
        /// </summary>
        /// <param name="listOptions"></param>
        void AddDropDownOptionsData(List<TMP_Dropdown.OptionData> listOptions)
        {
            ddlLanguage.AddOptions(listOptions);
        }

        /// <summary>
        /// 添加一个下拉数据
        /// </summary>
        /// <param name="itemText"></param>
        void AddDropDownOptionsData(string itemText)
        {
            //添加一个下拉选项
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            data.text = itemText;
            //data.image = "指定一个图片做背景不指定则使用默认"；
            ddlLanguage.options.Add(data);
        }

        /// <summary>
        /// 设置选择的下拉Item
        /// </summary>
        /// <param name="ItemIndex"></param>
        void SetDropDownItemValue(int ItemIndex)
        {
            if (ddlLanguage.options == null)
            {
                Debuger.Log(GetType() + "/SetDropDownItemValue()/下拉列表为空，请检查");
                return;
            }

            if (ItemIndex >= ddlLanguage.options.Count)
            {
                ItemIndex = ddlLanguage.options.Count - 1;
            }

            if (ItemIndex < 0)
            {
                ItemIndex = 0;
            }

            ddlLanguage.value = ItemIndex;
        }
    }
}
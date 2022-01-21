using System;
using System.Collections.Generic;
using Mega;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UILocalizationModel : BaseViewModel
    {
        private List<LocalizeData>            langList;
        private List<TMP_Dropdown.OptionData> optionList;

        public override void Init(Action onFinish = null)
        {
            initData();
            onFinish.Invoke();
        }

        private void initData()
        {
            langList   = Framework.L18N.GetSupportLanguages();
            optionList = new List<TMP_Dropdown.OptionData>();
            for (int i = 0; i < langList.Count; i++)
            {
                TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
                option.text = langList[i].langText;
                optionList.Add(option);
            }
        }

        public LocalizeData GetLanguage(int index)
        {
            if (langList == null)
            {
                initData();
            }

            return langList[index];
        }

        public List<TMP_Dropdown.OptionData> GetLanguageDropdownData()
        {
            if (optionList == null)
            {
                initData();
            }

            return optionList;
        }

        public int GetCurLangIndex()
        {
            for (int i = 0; i < langList.Count; i++)
            {
                if (langList[i].lang==Framework.L18N.CurLang)
                {
                    return i;
                }
            }

            return -1;
        }


        public override void Destroy()
        {
        }
    }
}
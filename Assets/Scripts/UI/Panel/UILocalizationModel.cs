using System;
using System.Collections.Generic;
using Mega;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UILocalizationModel : BaseViewModel
    {
        private List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();

        public override void Init(Action onFinish = null)
        {
            initData();
            onFinish.Invoke();
        }

        private void initData()
        {
            List<string> langList = Framework.L18N.GetSupportLanguages();
            for (int i = 0; i < langList.Count; i++)
            {
                Dropdown.OptionData option = new Dropdown.OptionData();
                option.text = langList[i];
                optionList.Add(option);
            }
        }

        private List<Dropdown.OptionData> GetLanguageDropdownData()
        {
            return optionList;
        }


        public override void Destroy()
        {
        }
    }
}
using System;
using System.Collections.Generic;
using Mega;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UILocalizationModel : BaseViewModel
    {
        private Dictionary<string, Localization> tableL18N;

        public override void Init(Action onFinish = null)
        {
            // tableL18N = Framework.Table.GetTable<Localization>();
        }

        private List<Dropdown.OptionData> GetLanguageDropdownData()
        {
            List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();
            List<string>              langList   = GetSupportLanguages();
            for (int i = 0; i < langList.Count; i++)
            {
                Dropdown.OptionData option = new Dropdown.OptionData();
                option.text = langList[i];
                optionList.Add(option);
            }

            return optionList;
        }

        private List<string> GetSupportLanguages()
        {
            List<string> list = new List<string>();
            list.Add(getString("语言", SystemLanguage.Chinese));
            list.Add(getString("语言", SystemLanguage.Japanese));
            list.Add(getString("语言", SystemLanguage.English));
            return list;
        }

        private string getString(string key, SystemLanguage language)
        {
            switch (language)
            {
                case SystemLanguage.Chinese:
                    return tableL18N[key].CN;
                case SystemLanguage.Japanese:
                    return tableL18N[key].JP;
                default:
                    return tableL18N[key].EN;
            }
        }

        private Sprite getImage(string key, SystemLanguage language)
        {
            string imgPath;
            switch (language)
            {
                case SystemLanguage.Chinese:
                    imgPath = $"Chinese/Sprite/{tableL18N[key].CN}";
                    break;
                case SystemLanguage.Japanese:
                    imgPath = $"Japanese/Sprite/{tableL18N[key].JP}";
                    break;
                default:
                    imgPath = $"English/Sprite/{tableL18N[key].EN}";
                    break;
            }

            return Resources.Load<Sprite>(imgPath);
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }
    }
}
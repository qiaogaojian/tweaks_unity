using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Mega
{
    [System.Serializable]
    public class Localization
    {
        // id
        public int id { get; set; }

        //语言
        public string KEY { get; set; }

        //中文
        public string CN { get; set; }

        //英语
        public string EN { get; set; }

        public Localization(Localization data)
        {
            id  = data.id;
            KEY = data.KEY;
            CN  = data.CN;
            EN  = data.EN;
        }

        public Localization()
        {
        }
    }

    public class localizationTable : ScriptableObject
    {
        public List<Localization> localizationList = new List<Localization>();
    }
}
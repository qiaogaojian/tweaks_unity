using System.Collections.Generic;
using UnityEngine;

namespace Mega
{
    public class DataManager : GameComponent
    {
        #region 数据表数据

        private readonly Dictionary<string, string> _dicLocalizationData = new Dictionary<string, string>();

        #endregion

        #region 加载数据表

        public void LoadCsvData()
        {
            this.CsvLocalizationData();
        }

        /// <summary>
        /// 加载本地化文件表
        /// </summary>
        private void CsvLocalizationData()
        {
             string csvPath  = Application.dataPath + "/Editor/CSV/1/localization.csv";
            Dictionary<int, Localization> localizationDic = CsvDataCached.CachedCsvFile<Localization>(csvPath);
            foreach (var data in localizationDic.Values)
            {
                this._dicLocalizationData.Add(data.KEY, data.CN);
            }

            localizationDic.Clear();
        }

        #endregion

        #region 获取数据表数据

        public string GetLocalization(string key)
        {
            if (this._dicLocalizationData.ContainsKey(key))
            {
                return this._dicLocalizationData[key];
            }

            return "error key";
        }

        #endregion

        #region 清理数据

        public void clearData()
        {
            this._dicLocalizationData.Clear();
        }

        #endregion
    }
}
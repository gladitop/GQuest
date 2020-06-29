using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminGQuest.Other
{
    static public class Data
    {
        #region Параметры

        public const string PathSave = "Settings.json";//Вот тут будет сохранятся настройки

        #endregion

        static public SettingsData Settings { get; set; }
    }
}

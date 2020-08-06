using Newtonsoft.Json;
using System.IO;

namespace AdminGQuest.Other
{
    public class SettingsData//Вот тут все настройки
    {
        [JsonProperty("login")]
        public string Login { get; set; }//Логин
        [JsonProperty("password")]
        public string Password { get; set; }//Пароль
    }

    public class SettingsManager//Для управление SettingsData
    {
        public SettingsManager(string pathsave)
        {
            Path = pathsave;
        }

        public string Path { get; set; }//Путь настроик

        public void Save(SettingsData data)
        {
            SettingsData i = new SettingsData { Login = data.Login, Password = data.Password };//Так надо!
            File.WriteAllText(Path, JsonConvert.SerializeObject(i));
        }

        public SettingsData Load()
        {
            if (!File.Exists(Path))
            {
                SettingsData i = new SettingsData { Login = null, Password = null };
                Save(i);
                return i;
            }
            else
            {
                SettingsData i = JsonConvert.DeserializeObject<SettingsData>(File.ReadAllText(Path));
                return i;
            }
        }
    }
}

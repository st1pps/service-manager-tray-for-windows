using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ServiceManager.Util
{
    public interface IAppSettings
    {
        void Save();

        List<string> Favorites { get; }

        bool ShowOnlyFavorites { get; set; }
    }

public class AppSettings : IAppSettings
    {
        private string _settingsFilePath;

        public static AppSettings LoadSettings(string settingsFilePath)
        {
            if (!File.Exists(settingsFilePath))
            {
                return new AppSettings(settingsFilePath);
            }

            var json = File.ReadAllText(settingsFilePath);
            var setting =  JsonConvert.DeserializeObject<AppSettings>(json, SerializerSettings);
            setting._settingsFilePath = settingsFilePath;
            return setting;
        }

        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        private AppSettings() { }

        private AppSettings(string settingsFilePath)
        {
            _settingsFilePath = settingsFilePath;
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(this, SerializerSettings);
            File.WriteAllText(_settingsFilePath, json);
        }

        [JsonIgnore]
        public List<string> Favorites
        {
            get
            {
                if (!AllFavorites.ContainsKey(Environment.MachineName))
                {
                    AllFavorites.Add(Environment.MachineName, new List<string>());
                    Save();
                }

                return AllFavorites[Environment.MachineName];
            }
        }

        public bool ShowOnlyFavorites { get; set;  }

        [JsonProperty]
        private Dictionary<string, List<string>> AllFavorites { get; } = new Dictionary<string, List<string>>();
    }
}

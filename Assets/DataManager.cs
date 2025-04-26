using Assembly_CSharp;
using System.IO;
using System.Text;
using UnityEngine;
using YG;

namespace Core
{
    public class DataManager : MonoBehaviour
    {
        public SettingsData userSettingsData;
        public SettingsData baseSettingsData;
        public UserData userData;
        [SerializeField] LanguageManager languageManager;
        [SerializeField] string configPath = Path.Combine(Application.streamingAssetsPath, "Config");

        public static DataManager Instance { get; set; }

        private void Awake()
        {
            Instance = this;

            // Подписываемся на событие загрузки данных
            YandexGame.LoadProgress();
            YandexGame.GetDataEvent += OnDataLoaded;

            // Загружаем базовые настройки (они не зависят от пользователя)
            LoadSettingsData(baseSettingsData, configPath);
            LoadLanguagesData();
        }

        private void OnDataLoaded()
        {
            // Загружаем пользовательские данные из YandexGame.savesData
            if (YandexGame.savesData.userSettingsJson != null)
            {
                JsonUtility.FromJsonOverwrite(YandexGame.savesData.userSettingsJson, userSettingsData);
            }
            else
            {
                // Если сохранений нет, загружаем базовые настройки
                LoadSettingsData(userSettingsData, configPath);
            }

            if (YandexGame.savesData.userDataJson != null)
            {
                JsonUtility.FromJsonOverwrite(YandexGame.savesData.userDataJson, userData);
            }
            else
            {
                // Если сохранений нет, загружаем начальные данные
                LoadUserData(userData, configPath);
            }
        }

        private void OnDestroy()
        {
            // Отписываемся от события при уничтожении объекта
            YandexGame.GetDataEvent -= OnDataLoaded;
        }

        public void SaveUserSettings()
        {
            SaveSettingsData(userSettingsData);
        }

        private void OnApplicationQuit()
        {
            SaveSettingsData(baseSettingsData, configPath);
            SaveSettingsData(userSettingsData);
            SaveUserData(userData);
        }

        [ContextMenu("Save settings data")]
        public void SaveSettingsData(SettingsData data, string path = null)
        {
            if (path != null)
            {
                // Сохраняем в файл (для базовых настроек)
                data.SaveToJson(path);
            }
            else
            {
                // Сохраняем в облако Яндекс Игр (для пользовательских настроек)
                YandexGame.savesData.userSettingsJson = JsonUtility.ToJson(data);
                YandexGame.SaveProgress();
            }
        }

        [ContextMenu("Load settings data")]
        public void LoadSettingsData(SettingsData data, string path)
        {
            data.LoadToJson(path);
        }

        [ContextMenu("Save user data")]
        public void SaveUserData(UserData data, string path = null)
        {
            if (path != null)
            {
                // Сохраняю в файл (для начальных данных)
                string json = JsonUtility.ToJson(data);
                if (File.Exists(Path.Combine(path, $"{data.name}.json")))
                {
                    File.WriteAllText(Path.Combine(path, $"{data.name}.json"), json);
                }
                else
                {
                    File.Create(Path.Combine(path, $"{data.name}.json"));
                    File.WriteAllText(Path.Combine(path, $"{data.name}.json"), json);
                }
            }
            else
            {
                // Сохраняю в облако Яндекс Игр
                YandexGame.savesData.userDataJson = JsonUtility.ToJson(data);
                YandexGame.SaveProgress();
            }
        }

        [ContextMenu("Load user data")]
        public void LoadUserData(UserData data, string path)
        {
            string json;
            if (File.Exists(Path.Combine(path, $"{data.name}.json")))
            {
                json = File.ReadAllText(Path.Combine(path, $"{data.name}.json"));
            }
            else
            {
                SaveUserData(data, path);
                json = File.ReadAllText(Path.Combine(path, $"{data.name}.json"));
            }
            JsonUtility.FromJsonOverwrite(json, data);
        }

        private void LoadLanguagesData()
        {
            string localizationJson = File.ReadAllText(Path.Combine(configPath, "Localization.json"), Encoding.UTF8);
            languageManager.localizationDictionary = JsonUtility.FromJson<LocalizationDictionary>(localizationJson);
        }
    }
}
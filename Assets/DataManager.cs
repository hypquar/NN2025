using Assembly_CSharp;
using System.IO;
using System.Text;
using UnityEngine;

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
            LoadSettingsData(baseSettingsData, configPath);
            LoadSettingsData(userSettingsData, configPath);
            LoadUserData(userData, configPath);
            LoadLanguagesData();
        }

        private void Start()
        {
            LoadSettingsData(baseSettingsData, configPath);
            LoadSettingsData(userSettingsData, configPath);
            LoadUserData(userData, configPath);
            LoadLanguagesData();
        }

        public void SaveUserSettings()
        {
            SaveSettingsData(userSettingsData, configPath);
        }

        private void OnApplicationQuit()
        {
            SaveSettingsData(baseSettingsData, configPath);
            SaveSettingsData(userSettingsData, configPath);
            SaveUserData(userData, configPath);
        }

        [ContextMenu("Save settings data")]
        public void SaveSettingsData(SettingsData data, string path)
        {
            data.SaveToJson(path);
        }

        [ContextMenu("Load settings data")]
        public void LoadSettingsData(SettingsData data, string path)
        {
            data.LoadToJson(path);
        }

        [ContextMenu("Save user data")]
        public void SaveUserData(UserData data, string path)
        {
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

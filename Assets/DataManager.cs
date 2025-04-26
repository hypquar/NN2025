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

            // ������������� �� ������� �������� ������
            YandexGame.LoadProgress();
            YandexGame.GetDataEvent += OnDataLoaded;

            // ��������� ������� ��������� (��� �� ������� �� ������������)
            LoadSettingsData(baseSettingsData, configPath);
            LoadLanguagesData();
        }

        private void OnDataLoaded()
        {
            // ��������� ���������������� ������ �� YandexGame.savesData
            if (YandexGame.savesData.userSettingsJson != null)
            {
                JsonUtility.FromJsonOverwrite(YandexGame.savesData.userSettingsJson, userSettingsData);
            }
            else
            {
                // ���� ���������� ���, ��������� ������� ���������
                LoadSettingsData(userSettingsData, configPath);
            }

            if (YandexGame.savesData.userDataJson != null)
            {
                JsonUtility.FromJsonOverwrite(YandexGame.savesData.userDataJson, userData);
            }
            else
            {
                // ���� ���������� ���, ��������� ��������� ������
                LoadUserData(userData, configPath);
            }
        }

        private void OnDestroy()
        {
            // ������������ �� ������� ��� ����������� �������
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
                // ��������� � ���� (��� ������� ��������)
                data.SaveToJson(path);
            }
            else
            {
                // ��������� � ������ ������ ��� (��� ���������������� ��������)
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
                // �������� � ���� (��� ��������� ������)
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
                // �������� � ������ ������ ���
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
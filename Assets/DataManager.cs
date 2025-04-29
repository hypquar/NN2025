using System.IO;
using System.Text;
using Assembly_CSharp;
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
        private string configPath;

        public static DataManager Instance { get; set; }

        private void Awake()
        {
            Instance = this;
            configPath = Path.Combine(Application.persistentDataPath, "Config");

            // ������� ����� ���� �� ����������
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }

            // ��������� ������� ���������
            LoadSettingsData(baseSettingsData, configPath);
            LoadLanguagesData();

            // ������������� �� ������� �������� ������
            YandexGame.GetDataEvent += OnDataLoaded;

            // ���� SDK ��� ��������, �������� �������
            if (YandexGame.SDKEnabled)
            {
                OnDataLoaded();
            }
            else
            {
                YandexGame.LoadProgress();
            }
        }

        public void Start()
        {
            // ��� ����� 
            if (!YandexGame.SDKEnabled)
            {
                userSettingsData.SetDefaultValues();
                userData.ResetData();
                Debug.Assert(userSettingsData != null, "UserSettingsData is not assigned in DataManager.");
            }
        }

        private void OnDataLoaded()
        {
            // ��������� ���������������� ������
            if (!string.IsNullOrEmpty(YandexGame.savesData.userSettingsJson))
            {
                JsonUtility.FromJsonOverwrite(YandexGame.savesData.userSettingsJson, userSettingsData);
            }
            else
            {
                userSettingsData.SetDefaultValues();
                SaveSettingsData(userSettingsData);
            }

            if (!string.IsNullOrEmpty(YandexGame.savesData.userDataJson))
            {
                JsonUtility.FromJsonOverwrite(YandexGame.savesData.userDataJson, userData);
            }
            else
            {
                userData.ResetData();
                SaveUserData(userData);
            }
        }

        public void AddCurrency(int amount)
        {
            if (amount <= 0) return;

            userData.currencyAmount += amount;
            SaveUserData(userData); // ��������� ���������
        }

        public bool SpendCurrency(int amount)
        {
            if (amount <= 0) return false;

            if (userData.currencyAmount >= amount)
            {
                userData.currencyAmount -= amount;
                SaveUserData(userData); // ��������� ���������
                return true;
            }
            else
            {
                Debug.LogWarning("�� ������� �����!");
                return false;
            }
        }

        public int GetCurrencyAmount()
        {
            return userData.currencyAmount;
        }



        public void SaveUserSettings()
        {
            SaveSettingsData(userSettingsData);
        }

        public void SaveSettingsData(SettingsData data, string path = null)
        {
            if (path != null)
            {
                data.SaveToJson(path);
            }
            else
            {
                YandexGame.savesData.userSettingsJson = JsonUtility.ToJson(data);
                YandexGame.SaveProgress();
            }
        }

        public void LoadSettingsData(SettingsData data, string path)
        {
            data.LoadToJson(path);
        }

        public void SaveUserData(UserData data, string path = null)
        {
            if (path != null)
            {
                string json = JsonUtility.ToJson(data);
                string filePath = Path.Combine(path, $"{data.name}.json");
                File.WriteAllText(filePath, json);
            }
            else
            {
                YandexGame.savesData.userDataJson = JsonUtility.ToJson(data);
                YandexGame.SaveProgress();
            }
        }

        public void LoadUserData(UserData data, string path)
        {
            string filePath = Path.Combine(path, $"{data.name}.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                JsonUtility.FromJsonOverwrite(json, data);
            }
            else
            {
                data.ResetData();
                SaveUserData(data, path);
            }
        }

        private void LoadLanguagesData()
        {
            string localizationPath = Path.Combine(Application.streamingAssetsPath, "config", "Localization.json");
            if (File.Exists(localizationPath))
            {
                string localizationJson = File.ReadAllText(localizationPath, Encoding.UTF8);
                languageManager.localizationDictionary = JsonUtility.FromJson<LocalizationDictionary>(localizationJson);
            }
            else
            {
                Debug.LogError("Localization file not found at: " + localizationPath);
                languageManager.localizationDictionary = new LocalizationDictionary();
            }
        }

        private void OnDestroy()
        {
            YandexGame.GetDataEvent -= OnDataLoaded;
        }
    }
}
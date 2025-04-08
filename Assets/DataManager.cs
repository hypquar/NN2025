using System.IO;
using UnityEngine;

namespace Core
{
    public class DataManager : MonoBehaviour
    {
        [SerializeField] SettingsData userSettingsData;
        [SerializeField] SettingsData baseSettingsData;
        [SerializeField] UserData userData;
        [SerializeField] LanguageManager languageManager;

        [ContextMenu("Save settings data")]
        public void SaveSettingsData(SettingsData data, string path)
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

        [ContextMenu("Load settings data")]
        public void LoadSettingsData(SettingsData data, string path)
        {
            string json;
            if (File.Exists(Path.Combine(path, $"{data.name}.json")))
            {
                json = File.ReadAllText(Path.Combine(path, $"{data.name}.json"));
            }
            else
            {
                SaveSettingsData(data, path);
                json = File.ReadAllText(Path.Combine(path, $"{data.name}.json"));
            }
            JsonUtility.FromJsonOverwrite(json, data);
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
    }
}

using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "NN2025/SettingsData")]
public class SettingsData : ScriptableObject
{
    public string Resolution = "1920x1080";
    public bool IsWindowed = true;
    public float musicVolume = 0.7f;
    public float sfxVolume = 0.7f;
    public string Language = "ru";

    public void SaveToJson(string path)
    {
        try
        {
            string json = JsonUtility.ToJson(this);
            string filePath = Path.Combine(path, $"{name}.json");

            // Создаем директорию если не существует
            Directory.CreateDirectory(path);

            File.WriteAllText(filePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save settings to {path}: {e.Message}");
        }
    }

    public void LoadToJson(string path)
    {
        string filePath = Path.Combine(path, $"{name}.json");

        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                JsonUtility.FromJsonOverwrite(json, this);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load settings from {path}: {e.Message}");
                SetDefaultValues();
            }
        }
        else
        {
            SetDefaultValues();
            SaveToJson(path);
        }
    }

    public void SetDefaultValues()
    {
        Resolution = "1920x1080";
        IsWindowed = true;
        musicVolume = 0.7f;
        sfxVolume = 0.7f;
        Language = "ru";
    }
}
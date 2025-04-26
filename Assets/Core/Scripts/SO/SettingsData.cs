using System.IO;
using UnityEngine;
using YG;

[CreateAssetMenu(fileName = "SettingsData", menuName = "NN2025/SettingsData")]
public class SettingsData : ScriptableObject
{
    public string Resolution;
    public bool IsWindowed;
    public float musicVolume;
    public float sfxVolume;
    public string Language;

    //Метод для облачного сохранения через Яндекс Игры
    public void SaveToCloud()
    {
        if (YandexGame.SDKEnabled)
        {
            YandexGame.savesData.userSettingsJson = JsonUtility.ToJson(this);
            YandexGame.SaveProgress();
        }
    }

    //Метод для загрузки из облака
    public void LoadFromCloud()
    {
        if (YandexGame.SDKEnabled && !string.IsNullOrEmpty(YandexGame.savesData.userSettingsJson))
        {
            JsonUtility.FromJsonOverwrite(YandexGame.savesData.userSettingsJson, this);
        }
    }

    // Сохранение в файл (оставлено для базовых настроек)
    public void SaveToJson(string path)
    {
        string json = JsonUtility.ToJson(this);

        try
        {
            File.WriteAllText(Path.Combine(path, $"{name}.json"), json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save settings: {e.Message}");
        }
    }

    // Загрузка из файла (оставлено для базовых настроек)
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
                Debug.LogError($"Failed to load settings: {e.Message}");
                SetDefaultValues();
            }
        }
        else
        {
            SetDefaultValues();
            SaveToJson(path);
        }
    }

    // Новый метод для установки значений по умолчанию
    public void SetDefaultValues()
    {
        Resolution = "1920x1080";
        IsWindowed = false;
        musicVolume = 0.7f;
        sfxVolume = 0.7f;
        Language = "ru";
    }
}
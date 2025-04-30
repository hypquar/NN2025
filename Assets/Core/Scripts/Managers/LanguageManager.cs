using Assembly_CSharp;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LanguageManager : MonoBehaviour, ISetting
{
    public UnityEvent<Dictionary<string, string>> OnLanguageChange;

    string localizationJson = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Config", "Localization.json"), Encoding.UTF8);
    [SerializeField] TMP_Dropdown dropdown;
    public LocalizationDictionary localizationDictionary = new LocalizationDictionary();
    Dictionary<string, string> currentLocalization = new Dictionary<string, string>();

    private void Awake()
    {
    }
    private void Start()
    {
        localizationDictionary.Russian = new List<LocalizationPair>();
        localizationDictionary.English = new List<LocalizationPair>();
        localizationDictionary = JsonUtility.FromJson<LocalizationDictionary>(localizationJson);
    }
    public void SaveSetting(SettingsData settingsData)
    {
        settingsData.Language = dropdown.options[dropdown.value].text;
    }
    public void LoadSetting(SettingsData settingsData)
    {
        for (int i = 0; i < dropdown.options.Count; i++)
        {
            if (dropdown.options[i].text == settingsData.Language)
            {
                dropdown.value = i;
                ChangeLanguage(dropdown.value);
            }
        }
    }

    public void ChangeLanguage(int value)
    {
        string languageTag = dropdown.options[value].text;
        SetCurrentLocalization(languageTag);
        OnLanguageChange.Invoke(currentLocalization);
    }

    void SetCurrentLocalization(string languageTag)
    {
        currentLocalization.Clear();
        if (languageTag == "Russian")
        {
            foreach (var pair in localizationDictionary.Russian)
            {
                currentLocalization.TryAdd(pair.Tag, pair.Value);
            }
        }
        if (languageTag == "English")
        {
            foreach (var pair in localizationDictionary.English)
            {
                currentLocalization.TryAdd(pair.Tag, pair.Value);
            }
        }
    }
}

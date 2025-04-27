using Assembly_CSharp;
using TMPro;
using UnityEngine;

public class ResolutionManager : MonoBehaviour, ISetting
{
    [SerializeField] TMP_Dropdown resolutionDropdown;
    
    private void Awake()
    {
    }
    public void SaveSetting(SettingsData settingsData)
    {
        settingsData.Resolution = resolutionDropdown.options[resolutionDropdown.value].text;
    }
    public void LoadSetting(SettingsData settingsData)
     {
        for (int i = 0; i < resolutionDropdown.options.Count; i++)
        {
            if (resolutionDropdown.options[i].text == settingsData.Resolution)
            {
                resolutionDropdown.value = i;
                ChangeResolution(resolutionDropdown.value);
            }
        }
    }
    public void ChangeResolution(int index)
    {
        string resolution = resolutionDropdown.options[index].text;
        string[] resolutionSplit = resolution.Split(':');
        int width = int.Parse(resolutionSplit[0]);
        int height = int.Parse(resolutionSplit[1]);
        Screen.SetResolution(width, height, Screen.fullScreenMode);
    }
}

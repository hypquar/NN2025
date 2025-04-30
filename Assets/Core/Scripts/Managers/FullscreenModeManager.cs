using Assembly_CSharp;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenModeManager : MonoBehaviour, ISetting
{
    [SerializeField] Toggle toggle;
    private void Awake()
    {
    }
    public void SaveSetting(SettingsData settingsData)
    {
        settingsData.IsWindowed = toggle.enabled;
    }
    public void LoadSetting(SettingsData settingsData)
    {
        toggle.isOn = settingsData.IsWindowed;
        ChangeFullscreenMode(settingsData.IsWindowed);
    }
    public void ChangeFullscreenMode(bool IsWindowed)
    {
        if (IsWindowed)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }
}

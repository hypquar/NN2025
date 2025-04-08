using Assembly_CSharp;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private SettingsData basicSettingsData;
    [SerializeField] private SettingsData userSettingsData;

    private ISetting[] settings;
    private void Awake()
    {
        settings = GetComponentsInChildren<ISetting>();
    }
    void Start()
    {
        foreach (ISetting setting in settings)
        {
            setting.SaveSetting(userSettingsData);
        }
    }
    public void Restore()
    {
        foreach (ISetting setting in settings)
        {
            setting.LoadSetting(basicSettingsData);
        }
    }
    public void Apply()
    {
        foreach (ISetting setting in settings)
        {
            setting.SaveSetting(userSettingsData);
        }
    }
    public void Revert()
    {
        foreach (ISetting setting in settings)
        {
            setting.LoadSetting(userSettingsData);
        }
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "NN2025/SettingsData")]
public class SettingsData : ScriptableObject
{
    public string Resolution;
    public bool IsWindowed;
    public bool IsSoundOn;
    public string Language;

    public void SaveToJson()
    {

    }

    public void LoadToJson()
    {

    }
}

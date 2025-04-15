using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "NN2025/SettingsData")]
public class SettingsData : ScriptableObject
{
    public string Resolution;
    public bool IsWindowed;
    public float musicVolume;
    public float sfxVolume;
    public string Language;

    public void SaveToJson(string path)
    {
        string json = JsonUtility.ToJson(this);
        if (File.Exists(Path.Combine(path, $"{this.name}.json")))
        {
            File.WriteAllText(Path.Combine(path, $"{this.name}.json"), json);
        }
        else
        {
            File.Create(Path.Combine(path, $"{this.name}.json"));
            File.WriteAllText(Path.Combine(path, $"{this.name}.json"), json);
        }
    }

    public void LoadToJson(string path)
    {
        string json;
        if (File.Exists(Path.Combine(path, $"{this.name}.json")))
        {
            json = File.ReadAllText(Path.Combine(path, $"{this.name}.json"));
        }
        else
        {
            SaveToJson(path);
            json = File.ReadAllText(Path.Combine(path, $"{this.name}.json"));
        }
        JsonUtility.FromJsonOverwrite(json, this);
    }
}

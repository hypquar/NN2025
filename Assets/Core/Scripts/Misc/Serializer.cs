using NUnit.Framework;
using System.IO;
using UnityEngine;

public class Serializer : MonoBehaviour
{
    [SerializeField] Locaaaal localizations;
    [SerializeField] string fileName;

    private void Start()
    {
        File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "Config", fileName), JsonUtility.ToJson(localizations, true));
    }

    [ContextMenu("Load")]
    void Load()
    {
        JsonUtility.FromJsonOverwrite(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Config", fileName)), localizations);
    }
}

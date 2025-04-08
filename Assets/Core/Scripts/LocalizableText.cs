using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
public class LocalizableText : MonoBehaviour
{
    [SerializeField] LanguageManager languageManager;
    [SerializeField] string keyWord;

    TextMeshProUGUI textMeshPro;
    UnityEvent<Dictionary<string, string>> languageChangeEvent;
    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        languageChangeEvent = languageManager.OnLanguageChange;
    }
    private void Start()
    {
        languageChangeEvent.AddListener(ChangeText);
    }
    private void ChangeText(Dictionary<string, string> dict)
    {
        foreach (var pair in dict)
        {
            if (dict.TryGetValue(keyWord, out string value))
            {
                textMeshPro.text = value;
                return;
            }
        }
    }
}

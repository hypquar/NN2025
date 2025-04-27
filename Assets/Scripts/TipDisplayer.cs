using System.Collections;
using TMPro;
using UnityEngine;

public class TipDisplayer : MonoBehaviour
{
    [SerializeField] float awaitTime = 2f;
    TextMeshProUGUI textMeshPro;
    string tipTextSaved;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }
    public void StartDisplay(string tipText)
    {
        tipTextSaved = tipText;
        StopDisplay();
        StartCoroutine(DisplayTip(tipText));
    }

    public void StopDisplay()
    {
        textMeshPro.text = "";
        StopAllCoroutines();
    }

    IEnumerator DisplayTip(string tipText)
    {
        yield return new WaitForSeconds(awaitTime);
        textMeshPro.text = tipText;
    }
}

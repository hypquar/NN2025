using UnityEngine;
using UnityEngine.UI;

public class ExitButtonComponent : MonoBehaviour
{
    Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    private void Start()
    {
        _button.onClick.AddListener(ExitApp);
    }
    void ExitApp()
    {
        Application.Quit();
    }
}

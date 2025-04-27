using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class BalanceButtonData
{
    public Button button;
    public int coinAmount;
}

public class CoinButtonController : MonoBehaviour
{
    [Header("Кнопки и их значения")]
    [SerializeField] private List<BalanceButtonData> balanceButtons;

    [Header("Скрипт анимации монет")]
    [SerializeField] private CoinFlyAnimationDOTween coinFX;

    private void Start()
    {
        if (coinFX == null || balanceButtons == null || balanceButtons.Count == 0)
        {
            Debug.LogWarning("Не подключены кнопки или FX.");
            return;
        }

        foreach (var data in balanceButtons)
        {
            var capturedData = data; // замыкание
            capturedData.button.onClick.AddListener(() =>
            {
                Vector3 pos = capturedData.button.transform.position;
                coinFX.PlayCoinAnimation(capturedData.coinAmount, pos);
            });
        }
    }
}

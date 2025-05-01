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
    [Header("Buttons Settings")]
    [SerializeField] private List<BalanceButtonData> balanceButtons;

    [Header("Coin Animation")]
    [SerializeField] private CoinFlyAnimationDOTween coinFX;

    private void Start()
    {
        if (coinFX == null || balanceButtons == null) return;

        foreach (var data in balanceButtons)
        {
            data.button.onClick.AddListener(() => OnButtonClick(data));
        }
    }

    private void OnButtonClick(BalanceButtonData data)
    {
        Vector3 worldPos = data.button.transform.position;
        coinFX.PlayCoinAnimation(data.coinAmount, worldPos);
    }
}
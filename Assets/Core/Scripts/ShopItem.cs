using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    [Header("Shop Items")]
    [SerializeField] private List<ShopItemData> shopItems = new List<ShopItemData>();

    [Header("Colors")]
    [SerializeField] private Color affordableColor = Color.green;
    [SerializeField] private Color unaffordableColor = Color.red;

    private void Start()
    {
        Wallet.Instance.OnCurrencyChanged += UpdateAllItemsUI;
        UpdateAllItemsUI();
    }

    private void OnDestroy()
    {
        if (Wallet.Instance != null)
        {
            Wallet.Instance.OnCurrencyChanged -= UpdateAllItemsUI;
        }
    }

    private void UpdateAllItemsUI()
    {
        foreach (var item in shopItems)
        {
            UpdateItemUI(item);
        }
    }

    private void UpdateItemUI(ShopItemData item)
    {
        if (Wallet.Instance == null) return;

        bool canBuy = Wallet.Instance.GetCurrencyAmount() >= item.itemPrice;
        item.priceText.color = canBuy ? affordableColor : unaffordableColor;
        item.priceText.text = item.itemPrice.ToString();
    }

    private void TryBuyItem(ShopItemData item)
    {
        if (Wallet.Instance.SpendCurrency(item.itemPrice))
        {
            Debug.Log("Item purchased!");
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{


    [Header("Товары магазина")]
    [SerializeField] private List<ShopItemData> shopItems = new List<ShopItemData>();

    [Header("Цвета подсветки")]
    [SerializeField] private Color affordableColor = Color.green;
    [SerializeField] private Color unaffordableColor = Color.red;

    private void Start()
    {
        foreach (var item in shopItems)
        {
            if (item.buyButton != null)
            {
                item.buyButton.onClick.AddListener(() => TryBuyItem(item));
            }
        }

        if (Wallet.Instance != null)
        {
            Wallet.Instance.OnCurrencyChanged += UpdateAllItemsUI;
        }

        UpdateAllItemsUI(); // обновим UI при старте
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

        if (item.priceText != null)
        {
            item.priceText.color = canBuy ? affordableColor : unaffordableColor;
        }

        if (item.priceText != null)
        {
            item.priceText.text = item.itemPrice.ToString();
        }
    }

    private void TryBuyItem(ShopItemData item)
    {
        if (Wallet.Instance == null) return;

        if (Wallet.Instance.SpendCurrency(item.itemPrice))
        {
            Debug.Log("Товар куплен за " + item.itemPrice + " монет!");
            // Здесь можно добавить выдачу предмета игроку
        }
        else
        {
            Debug.LogWarning("Недостаточно монет для покупки товара за " + item.itemPrice + "!");
        }
    }
}

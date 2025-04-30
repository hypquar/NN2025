using UnityEngine;
using TMPro;
using System;
using Core; 

public class Wallet : MonoBehaviour
{
    public static Wallet Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI coinText;

    private int _currencyAmount;

    // === Событие изменения баланса ===
    public event Action OnCurrencyChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Чтобы не терять кошелек между сценами
    }

    private void Start()
    {
        LoadCurrency();
        UpdateUI();
        OnCurrencyChanged.Invoke();
    }

    public void AddCurrency(int amount)
    {
        if (amount <= 0) return;

        _currencyAmount += amount;
        SaveCurrency();
        UpdateUI();
        OnCurrencyChanged?.Invoke(); // ВАЖНО: вызываем событие
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= 0) return false;

        if (_currencyAmount >= amount)
        {
            _currencyAmount -= amount;
            SaveCurrency();
            UpdateUI();
            OnCurrencyChanged?.Invoke(); // ВАЖНО: вызываем событие
            return true;
        }
        else
        {
            Debug.LogWarning("Не хватает монет!");
            return false;
        }
    }

    public int GetCurrencyAmount()
    {
        return _currencyAmount;
    }

    private void SaveCurrency()
    {
        DataManager.Instance.userData._currencyAmount = _currencyAmount;
        DataManager.Instance.SaveUserData(DataManager.Instance.userData);
    }

    private void LoadCurrency()
    {
        _currencyAmount = DataManager.Instance.userData._currencyAmount;
    }

    private void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = _currencyAmount.ToString();
        }
    }
}

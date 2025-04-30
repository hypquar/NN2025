using UnityEngine;
using TMPro;
using System; // нужно для события
using Core; 

public class Wallet : MonoBehaviour
{
    public static Wallet Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI coinText;

    private int currencyAmount;

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
    }

    public void AddCurrency(int amount)
    {
        if (amount <= 0) return;

        currencyAmount += amount;
        SaveCurrency();
        UpdateUI();
        OnCurrencyChanged?.Invoke(); // ВАЖНО: вызываем событие
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= 0) return false;

        if (currencyAmount >= amount)
        {
            currencyAmount -= amount;
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
        return currencyAmount;
    }

    private void SaveCurrency()
    {
        DataManager.Instance.userData.currencyAmount = currencyAmount;
        DataManager.Instance.SaveUserData(DataManager.Instance.userData);
    }

    private void LoadCurrency()
    {
        currencyAmount = DataManager.Instance.userData.currencyAmount;
    }

    private void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = currencyAmount.ToString();
        }
    }
}

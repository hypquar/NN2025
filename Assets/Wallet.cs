using UnityEngine;
using TMPro;
using Core; // подключаем чтобы использовать DataManager

public class Wallet : MonoBehaviour
{

    public static Wallet Instance { get; private set; } // синглтон

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI coinText;
    private int currencyAmount;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // если кошелек уже существует — уничтожаем копию
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // опционально: сохранять кошелек при смене сцен
    }
    private void Start()
    {

        LoadCurrency(); // при старте загружаем из сохранения
        UpdateUI();
    }

    public void AddCurrency(int amount)
    {
        if (amount <= 0) return;

        currencyAmount += amount;
        SaveCurrency();
        UpdateUI();
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= 0) return false;

        if (currencyAmount >= amount)
        {
            currencyAmount -= amount;
            SaveCurrency();
            UpdateUI();
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
        // сохраняем в DataManager и сразу в YandexGame
        DataManager.Instance.userData.currencyAmount = currencyAmount;
        DataManager.Instance.SaveUserData(DataManager.Instance.userData);
    }

    private void LoadCurrency()
    {
        // загружаем из DataManager
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

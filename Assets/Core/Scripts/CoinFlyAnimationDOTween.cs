using UnityEngine;
using TMPro;
using DG.Tweening;

// Примечание: Не забудьте добавить ссылку на Wallet в инспекторе Unity, чтобы избежать ошибок во время выполнения.
// Примечание: Убедитесь, что у вас есть префаб монеты и эффект вспышки, назначенные в инспекторе.
// Примечание: Убедитесь, что у вас есть ссылка на AudioSource в инспекторе Unity.
// Примечание: Убедитесь, что у вас есть ссылка на Canvas в инспекторе Unity.
// Примечание: Убедитесь, что у вас есть ссылка на TextMeshProUGUI в инспекторе Unity.  
public class CoinFlyAnimationDOTween : MonoBehaviour
{
    [Header("Настройки монет")]
    public GameObject coinPrefab;
    public RectTransform coinTarget;
    public RectTransform canvas;
    public TextMeshProUGUI coinDisplay;

    [Header("Анимация")]
    public float totalAnimationDuration = 3f;
    public float coinFlyTime = 0.6f;

    [Header("Эффекты")]
    public AudioClip coinSound;
    public GameObject flashEffectPrefab;
    public AudioSource audioSource;

    public int maxCoinsToAnimate = 100;

    [Header("Кошелек игрока")]
    public Wallet wallet; // <-- добавляем ссылку на Wallet

    private void Start()
    {
        if (wallet == null)
        {
            Debug.LogError("Не назначен Wallet в CoinFlyAnimationDOTween!");
        }
    }

    public void PlayCoinAnimation(int coinAmount, Vector3 startWorldPosition)
    {
        int coinsToAnimate = Mathf.Min(coinAmount, maxCoinsToAnimate);
        float valuePerCoin = (float)coinAmount / coinsToAnimate;

        float spawnTimeWindow = totalAnimationDuration - coinFlyTime;
        spawnTimeWindow = Mathf.Max(spawnTimeWindow, 0.01f);

        float accumulated = 0f;

        PlayCoinSound();

        for (int i = 0; i < coinsToAnimate; i++)
        {
            float delay = (spawnTimeWindow / coinsToAnimate) * i;

            accumulated += valuePerCoin;
            int valueToAdd = Mathf.FloorToInt(accumulated);

            accumulated -= valueToAdd;

            if (i == coinsToAnimate - 1)
            {
                valueToAdd += Mathf.RoundToInt(accumulated);
            }

            SpawnCoinWithDelay(startWorldPosition, delay, valueToAdd);
        }
    }

    private void SpawnCoinWithDelay(Vector3 worldStartPos, float delay, int valueToAdd)
    {
        GameObject coin = Instantiate(coinPrefab, canvas.transform);
        RectTransform coinRT = coin.GetComponent<RectTransform>();
        coinRT.sizeDelta = new Vector2(50f, 50f);

        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldStartPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas, screenPos, canvas.GetComponent<Canvas>().worldCamera, out Vector2 localPos);
        coinRT.anchoredPosition = localPos;
        coinRT.localScale = Vector3.zero;

        Vector2 randomOffset = new Vector2(Random.Range(-50f, 50f), Random.Range(50f, 100f));
        Vector2 midPoint = (coinTarget.anchoredPosition + localPos) / 2 + randomOffset;

        Sequence s = DOTween.Sequence();
        s.AppendInterval(delay);
        s.AppendCallback(() => coinRT.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
        s.Append(coinRT.DOAnchorPos(midPoint, coinFlyTime / 2).SetEase(Ease.OutQuad));
        s.Append(coinRT.DOAnchorPos(coinTarget.anchoredPosition, coinFlyTime / 2).SetEase(Ease.InQuad));
        s.Join(coinRT.DOScale(0.3f, coinFlyTime).SetEase(Ease.InQuad));

        s.OnComplete(() =>
        {
            Destroy(coin);
            wallet.AddCurrency(valueToAdd); // <-- Добавляем через кошелек

            if (flashEffectPrefab != null)
            {
                GameObject flash = Instantiate(flashEffectPrefab, coinTarget);
                Destroy(flash, 1f);
            }
        });
    }

    private void PlayCoinSound()
    {
        if (audioSource != null && coinSound != null)
        {
            audioSource.PlayOneShot(coinSound);
        }
    }
}
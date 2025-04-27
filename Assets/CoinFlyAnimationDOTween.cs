using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

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

    private int currentCoinCount = 0;

    public int maxCoinsToAnimate = 100;

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
            int valueToAdd = Mathf.FloorToInt(accumulated); // только целая часть

            accumulated -= valueToAdd; // оставим остаток на следующую монету

            // если последняя монетка — прибавим остаток полностью
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
            currentCoinCount += valueToAdd;
            coinDisplay.text = currentCoinCount.ToString();

            if (flashEffectPrefab != null)
            {
                GameObject flash = Instantiate(flashEffectPrefab, coinTarget);
                Destroy(flash, 1f);
            }
        });
    }

    // Метод для проигрывания звука

    private void PlayCoinSound()
    {
        if (audioSource != null && coinSound != null)
        {
            audioSource.PlayOneShot(coinSound);
        }
    }




    private IEnumerator SpawnCoinsRoutine(int amount, Vector3 worldStartPos)
    {
        float delayBetweenCoins = (totalAnimationDuration - coinFlyTime) / amount;
        delayBetweenCoins = Mathf.Max(delayBetweenCoins, 0f);

        for (int i = 0; i < amount; i++)
        {
            SpawnCoin(worldStartPos);
            yield return new WaitForSeconds(delayBetweenCoins);
        }
    }

    private void SpawnCoin(Vector3 worldStartPos)
    {
        GameObject coin = Instantiate(coinPrefab, canvas.transform);
        RectTransform coinRT = coin.GetComponent<RectTransform>();
        coinRT.sizeDelta = new Vector2(50f, 50f);

        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldStartPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas, screenPos, canvas.GetComponent<Canvas>().worldCamera, out Vector2 localPos);
        coinRT.anchoredPosition = localPos;
        coinRT.localScale = Vector3.zero;

        coinRT.DOScale(1f, 0.2f).SetEase(Ease.OutBack);

        Vector2 randomOffset = new Vector2(Random.Range(-50f, 50f), Random.Range(50f, 100f));
        Vector2 midPoint = (coinTarget.anchoredPosition + localPos) / 2 + randomOffset;

        Sequence s = DOTween.Sequence();
        s.Append(coinRT.DOAnchorPos(midPoint, coinFlyTime / 2).SetEase(Ease.OutQuad));
        s.Append(coinRT.DOAnchorPos(coinTarget.anchoredPosition, coinFlyTime / 2).SetEase(Ease.InQuad));
        s.Join(coinRT.DOScale(0.3f, coinFlyTime).SetEase(Ease.InQuad));

        s.OnComplete(() =>
        {
            Destroy(coin);
            currentCoinCount += 1;
            coinDisplay.text = currentCoinCount.ToString();

            //  Вспышка
            if (flashEffectPrefab != null)
            {
                GameObject flash = Instantiate(flashEffectPrefab, coinTarget);
                Destroy(flash, 1f); // удалим вспышку через секунду
            }
        });
    }
}


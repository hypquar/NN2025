using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinFlyAnimationDOTween : MonoBehaviour
{
    [Header("������")]
    [SerializeField] private RectTransform coinTarget;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text coinDisplay;

    [Header("��������� ��������")]
    [SerializeField] private int coinCount = 10;
    [SerializeField] private float spawnDelay = 0.05f;
    [SerializeField] private float flyTime = 0.6f;
    [SerializeField] private Vector2 arcOffset = new Vector2(0, 100f);
    [SerializeField] private Ease easeType = Ease.OutQuad;

    private int currentCoins = 0;

public void PlayCoinAnimation(Vector3 fromWorldPos, int coinAmount)
{
    StartCoroutine(SpawnCoins(fromWorldPos, coinAmount));
}

private IEnumerator SpawnCoins(Vector3 fromPos, int coinAmount)
{
    for (int i = 0; i < coinAmount; i++)
    {
        SpawnCoin(fromPos);
        yield return new WaitForSeconds(spawnDelay);
    }

    yield return new WaitForSeconds(flyTime + 0.1f);
    currentCoins += coinAmount;
    coinDisplay.text = currentCoins.ToString();
}


    private void SpawnCoin(Vector3 worldStartPos)
    {
        // ������ ������ � �������� UI
        GameObject coin = Instantiate(coinPrefab, canvas.transform);
        RectTransform coinRT = coin.GetComponent<RectTransform>();

        // ����������� ������� ������� (�� world space) � ��������� ������� ������������ Canvas
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldStartPos); // ��������� � �������� ����������
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            canvas.worldCamera,
            out Vector2 localPos
        );

        // ������������� ������� ������ � Canvas
        coinRT.anchoredPosition = localPos;
        coinRT.localScale = Vector3.zero;  // ��������� ������ ������

        // ��������� �������� ���������� (���������)
        coinRT.DOScale(1f, 0.2f).SetEase(Ease.OutBack);

        // ����� �������� �����
        Vector2 randomOffset = new Vector2(Random.Range(-50f, 50f), Random.Range(50f, 100f));  // ��� ���������� �������
        Vector2 midPoint = (coinTarget.anchoredPosition + localPos) / 2 + randomOffset;  // �������� ���� � ��������� �����

        Sequence s = DOTween.Sequence();
        s.Append(coinRT.DOAnchorPos(midPoint, flyTime / 2).SetEase(Ease.OutQuad));
        s.Append(coinRT.DOAnchorPos(coinTarget.anchoredPosition, flyTime / 2).SetEase(Ease.InQuad));
        s.Join(coinRT.DOScale(0.3f, flyTime).SetEase(Ease.InQuad));  // �������� ����������

        // ����� �������� ���������, ���������� ������
        s.OnComplete(() => Destroy(coin));
    }
}

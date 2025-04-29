using UnityEngine;
using DG.Tweening;

public class DOTweenInit : MonoBehaviour
{
    [Header("DOTween Settings")]
    [Tooltip("Максимум одновременно активных твинов")]
    public int tweenCapacity = 1000;

    [Tooltip("Максимум одновременно активных последовательностей (Sequences)")]
    public int sequenceCapacity = 250;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DOTween.SetTweensCapacity(tweenCapacity, sequenceCapacity);
        Debug.Log($"<color=#00c6ff><b>DOTween ▶</b></color> Tweens capacity set to {tweenCapacity}/{sequenceCapacity}");
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] float awaitTime;
    public UnityEvent OnTimerStart;
    public UnityEvent<float> OnTimerUpdate;
    public UnityEvent OnTimerStop;

    private void OnEnable()
    {
        OnTimerStart.Invoke();
        StartCoroutine(TimerUpdate());
    }

    private void OnDisable()
    {
        OnTimerStop.Invoke();
        StopAllCoroutines();
    }
    IEnumerator TimerUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
        }
    }

}

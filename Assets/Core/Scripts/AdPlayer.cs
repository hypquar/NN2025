using Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class AdPlayer : MonoBehaviour
{
    public UnityEvent OnAdEnd;
    GameObject adTrigger;
    VideoPlayer adPlayer;
    float adDuration;
    private void Awake()
    {
        adPlayer = GetComponentInChildren<VideoPlayer>();
    }

    private void OnEnable()
    {
        adDuration = (float)adPlayer.length;
        StartCoroutine(AdTimer(adDuration));
        OnAdEnd.AddListener(StartTriggerCooldown);
    }

    private void StartTriggerCooldown()
    {
        if(adTrigger.TryGetComponent<TimerComponent>(out TimerComponent timer))
        {
            timer.enabled = true;
        }
        if(adTrigger.TryGetComponent<Button>(out Button button))
        {
            button.enabled = false;
        }
    }

    IEnumerator AdTimer(float time)
    {
        yield return new WaitForSeconds(time);
        OnAdEnd.Invoke();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void GetAdTrigger(GameObject trigger)
    {
        adTrigger = trigger;
    }
}

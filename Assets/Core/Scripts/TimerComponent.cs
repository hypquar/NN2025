using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class TimerComponent : MonoBehaviour
    {
        public UnityEvent OnTimerStart;
        public UnityEvent OnAlarm;
        public UnityEvent<float> OnTimeUpdate;
        [SerializeField] TimerData timerData;
        [SerializeField] float increment = 1f;
        Coroutine timerCoroutine;

        public float Time
        {
            get
            {
                return timerData.timeCurrent;
            }
            private set
            {
                if (value <= 0)
                {
                    timerData.timeCurrent = 0;
                    OnTimeUpdate.Invoke(timerData.timeCurrent);
                    OnAlarm.Invoke();
                }
                else
                {
                    timerData.timeCurrent = value;
                    OnTimeUpdate.Invoke(timerData.timeCurrent);
                }
            }
        }
        public void StartTimer()
        {
            ResetTimer();
            timerCoroutine = StartCoroutine(TimeUpdate());
            OnTimerStart.Invoke();
            OnAlarm.AddListener(Stop);
        }
        public void Stop()
        {
            StopAllCoroutines();
            ResetTimer();
            OnTimeUpdate.Invoke(timerData.timeCurrent);
        }
        public void ResetTimer()
        {
            timerData.timeCurrent = timerData.timeSet;
        }
        IEnumerator TimeUpdate()
        {
            while (timerData.timeCurrent >= 0)
            {
                yield return new WaitForSecondsRealtime(increment);
                OnTimeUpdate.Invoke(timerData.timeCurrent);
                timerData.timeCurrent -= increment;
                Time = timerData.timeCurrent;
            }
        }
    }
}

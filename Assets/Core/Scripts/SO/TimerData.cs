using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    [CreateAssetMenu(fileName = "TimerData", menuName = "NN2025/TimerData")]
    public class TimerData : ScriptableObject
    {
        public float timeSet;
        public float timeCurrent;
    }
}

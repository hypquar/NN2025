using TMPro;
using UnityEngine;

namespace Core
{
    public class TimerDisplay : MonoBehaviour
    {
        [SerializeField] bool isFormated;
        [SerializeField] bool dependsOnVisibility;
        [SerializeField] TimerComponent timer;
        TextMeshProUGUI textField;
        private void OnEnable()
        {
            textField = GetComponent<TextMeshProUGUI>();
            timer.OnTimeUpdate.AddListener(Display);
        }

        private void OnDisable()
        {
        }

        private void Start()
        {
            if (isFormated)
            {
                if (timer.Time / 60 < 10)
                {
                    if (timer.Time % 60 < 10)
                    {
                        textField.text = $"0{(int)timer.Time / 60}:0{timer.Time % 60}";
                    }
                    else
                    {
                        textField.text = $"0{(int)timer.Time / 60}:{timer.Time % 60}";
                    }
                }
                else
                {
                    if (timer.Time % 60 < 10)
                    {
                        textField.text = $"{(int)timer.Time / 60}:0{timer.Time % 60}";
                    }
                    else
                    {
                        textField.text = $"{(int)timer.Time / 60}:{timer.Time % 60}";
                    }
                }
            }
            else
            {
                textField.text = timer.Time.ToString();
            }
        }
        public void Display(float value)
        {
            if (isFormated)
            {
                if ((int)value / 60 < 10)
                {
                    if (value % 60 < 10)
                    {
                        textField.text = $"0{(int)value / 60}:0{value % 60}";
                    }
                    else
                    {
                        textField.text = $"0{(int)value / 60}:{value % 60}";
                    }
                }
                else
                {
                    if (value % 60 < 10)
                    {
                        textField.text = $"{(int)value / 60}:0{value % 60}";
                    }
                    else
                    {
                        textField.text = $"{(int)value / 60}:{value % 60}";
                    }
                }
            }
            else
            {
                textField.text = value.ToString();
            }
        }

        private void StopDisplay()
        {
            timer.OnTimeUpdate.RemoveListener(Display);
        }
    }
}

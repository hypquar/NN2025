using UnityEngine;

public class WinAreaComponent : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _levelManager.OnLevelComplete.Invoke();
    }
}

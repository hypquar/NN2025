using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public UnityEvent OnLevelComplete;
    public UnityEvent<int> OnLevelUpdate;
    [SerializeField] private UserData _userData;
    [SerializeField] private TextMeshProUGUI _levelDisplay;

    public int LevelId
    {
        get
        {
            return _userData.lastFinishedLevelIndex;
        }
        private set
        {
            _userData.lastFinishedLevelIndex = value;
            OnLevelUpdate.Invoke(_userData.lastFinishedLevelIndex);
        }
    }

    private void Awake()
    {
        OnLevelUpdate.AddListener(DisplayLevel);
        OnLevelComplete.AddListener(LoadNextLevel);
    }

    private void Start()
    {
        OnLevelUpdate.Invoke(_userData.lastFinishedLevelIndex);
    }

    private void DisplayLevel(int id)
    {
        _levelDisplay.text = $"LEVEL {id}";
    }

    private void LoadNextLevel()
    {
        LevelId += 1;
    }
}

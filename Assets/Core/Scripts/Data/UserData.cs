using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "Scriptable Objects/UserData")]
public class UserData : ScriptableObject
{
    public int lastFinishedLevelIndex;
    public int _currencyAmount;

    //����� ������ ���������� 
    public void ResetData()
    {
        lastFinishedLevelIndex = 0;
        _currencyAmount = 0;
    }
}

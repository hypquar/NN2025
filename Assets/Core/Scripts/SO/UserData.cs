using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "Scriptable Objects/UserData")]
public class UserData : ScriptableObject
{
    public int lastFinishedLevelIndex;
    public int currencyAmount;

    //����� ������ ���������� 
    public void ResetData()
    {
        lastFinishedLevelIndex = 0;
        currencyAmount = 0;
    }
}

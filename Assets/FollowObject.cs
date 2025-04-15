using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] Transform target;
    private Vector3 lastTargetPosition;

    void Start()
    {
        if (target != null)
        {
            lastTargetPosition = target.position;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // ��������� �������� ����
        Vector3 delta = target.position - lastTargetPosition;

        // ���������� ������ �� ����� �� ��������
        transform.position += delta;

        // ��������� ��������� �������
        lastTargetPosition = target.position;
    }
}

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

        // Вычисляем смещение цели
        Vector3 delta = target.position - lastTargetPosition;

        // Перемещаем камеру на такое же смещение
        transform.position += delta;

        // Обновляем последнюю позицию
        lastTargetPosition = target.position;
    }
}

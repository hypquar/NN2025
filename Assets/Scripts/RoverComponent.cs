using UnityEngine;

public class RoverComponent : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _body2d;
    [SerializeField] private WheelJoint2D[] _wheels;

    public void StartEngine()
    {
        foreach (var wheel in _wheels)
        {
            wheel.useMotor = true;
        }
    }

    public void Jump()
    {
        _body2d.AddForce(Vector2.up * 100 + Vector2.right * 50, ForceMode2D.Impulse);
    }
}

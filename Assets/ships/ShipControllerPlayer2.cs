
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipControllerPlayer2 : MonoBehaviour
{
    private Rigidbody spaceshipRB;

    [Header("Параметры тяги и крутящего момента")]
    [Tooltip("Ускорение вдоль оси вперёд/назад ")]
    public float thrustAcceleration = 30f;

    [Tooltip("Угловое ускорение для наклона")]
    public float pitchAcceleration = 10f;

    [Tooltip("Угловое ускорение для крена")]
    public float rollAcceleration = 10f;

    [Tooltip("Угловое ускорение для рыскания ")]
    public float yawAcceleration = 10f;

    [Header("Настройки сопротивления")]
    [Tooltip("Линейное сопротивление ")]
    [Range(0f, 5f)]
    public float linearDrag = 0.2f;

    [Tooltip("Угловое сопротивление")]
    [Range(0f, 5f)]
    public float angularDrag = 0.2f;

    [Header("Ограничения скорости ")]
    [Tooltip("Максимальная линейная скорость ")]
    public float maxLinearSpeed = 50f;

    [Tooltip("Максимальная угловая скорость")]
    public float maxAngularSpeed = 10f;

    [Range(0f, 20f)]
    public float stabilizationFactor = 5f;

    private void Awake()
    {
        spaceshipRB = GetComponent<Rigidbody>();

        spaceshipRB.linearDamping = linearDrag;
        spaceshipRB.angularDamping = angularDrag;
        spaceshipRB.mass = 1f;

        if (maxAngularSpeed > 0f)
            spaceshipRB.maxAngularVelocity = maxAngularSpeed;
    }

    private void FixedUpdate()
    {
        float thrustInput = 0f;
        if (Input.GetKey(KeyCode.RightShift)) thrustInput = +1f;
        else if (Input.GetKey(KeyCode.RightControl)) thrustInput = -1f;

        if (thrustInput != 0f)
        {
            Vector3 force = transform.forward * (thrustInput * thrustAcceleration);
            spaceshipRB.AddForce(force, ForceMode.Acceleration);
        }

        float pitchInput = 0f;
        if (Input.GetKey(KeyCode.Keypad8)) pitchInput = +1f;
        else if (Input.GetKey(KeyCode.Keypad5)) pitchInput = -1f;

        if (pitchInput != 0f)
        {
            Vector3 pitchAxis = transform.right;
            Vector3 pitchTorqueVec = pitchAxis * (pitchInput * pitchAcceleration);
            spaceshipRB.AddTorque(pitchTorqueVec, ForceMode.Acceleration);
        }

        float rollInput = 0f;
        if (Input.GetKey(KeyCode.Keypad4)) rollInput = +1f;
        else if (Input.GetKey(KeyCode.Keypad6)) rollInput = -1f;

        if (rollInput != 0f)
        {
            Vector3 rollAxis = transform.forward;
            Vector3 rollTorqueVec = rollAxis * (rollInput * rollAcceleration);
            spaceshipRB.AddTorque(rollTorqueVec, ForceMode.Acceleration);
        }

        float yawInput = 0f;
        if (Input.GetKey(KeyCode.Keypad9)) yawInput = +1f;
        else if (Input.GetKey(KeyCode.Keypad7)) yawInput = -1f;

        if (yawInput != 0f)
        {
            Vector3 yawAxis = transform.up;
            Vector3 yawTorqueVec = yawAxis * (yawInput * yawAcceleration);
            spaceshipRB.AddTorque(yawTorqueVec, ForceMode.Acceleration);
        }

        Vector3 currentAngVel = spaceshipRB.angularVelocity;
        if (currentAngVel.magnitude > 0.001f)
        {
            Vector3 stabilizingTorque = -currentAngVel * stabilizationFactor;
            spaceshipRB.AddTorque(stabilizingTorque, ForceMode.Acceleration);
        }

        if (maxLinearSpeed > 0f && spaceshipRB.linearVelocity.magnitude > maxLinearSpeed)
        {
            spaceshipRB.linearVelocity = spaceshipRB.linearVelocity.normalized * maxLinearSpeed;
        }
    }
}
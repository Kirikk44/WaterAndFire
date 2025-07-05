using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceshipPhysicsController : MonoBehaviour
{
    private Rigidbody spaceshipRB;

    [Header("Параметры тяги и крутящего момента")]
    [Tooltip("Ускорение вдоль оси вперёд/назад ")]
    public float thrustAcceleration = 30f;

    [Tooltip("Угловое ускорение для наклона ")]
    public float pitchAcceleration = 10f;

    [Tooltip("Угловое ускорение для крена")]
    public float rollAcceleration = 10f;

    [Tooltip("Угловое ускорение для рыскания ")]
    public float yawAcceleration = 10f;

    [Header("Настройки сопротивления")]
    [Tooltip("Линейное сопротивление")]
    [Range(0f, 5f)]
    public float linearDrag = 0.2f;

    [Tooltip("Угловое сопротивление")]
    [Range(0f, 5f)]
    public float angularDrag = 0.2f;

    [Header("Ограничения скорости ")]
    [Tooltip("Максимальная линейная скорость ")]
    public float maxLinearSpeed = 50f;

    [Tooltip("Максимальная угловая скорость ")]
    public float maxAngularSpeed = 10f;

    [Header("Стабилизация ")]
    [Tooltip("Коэффициент")]
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
        if (Input.GetKey(KeyCode.LeftShift)) thrustInput = +1f;
        else if (Input.GetKey(KeyCode.LeftControl)) thrustInput = -1f;

        if (thrustInput != 0f)
        {
            Vector3 force = transform.forward * (thrustInput * thrustAcceleration);
            spaceshipRB.AddForce(force, ForceMode.Acceleration);
        }

        float pitchInput = 0f;
        if (Input.GetKey(KeyCode.W)) pitchInput = +1f;
        else if (Input.GetKey(KeyCode.S)) pitchInput = -1f;

        if (pitchInput != 0f)
        {
            Vector3 pitchAxis = transform.right;
            Vector3 pitchTorqueVec = pitchAxis * (pitchInput * pitchAcceleration);
            spaceshipRB.AddTorque(pitchTorqueVec, ForceMode.Acceleration);
        }

        // 3) Roll (крен) — клавиши A / D
        float rollInput = 0f;
        if (Input.GetKey(KeyCode.A)) rollInput = +1f;
        else if (Input.GetKey(KeyCode.D)) rollInput = -1f;

        if (rollInput != 0f)
        {
            Vector3 rollAxis = transform.forward;
            Vector3 rollTorqueVec = rollAxis * (rollInput * rollAcceleration);
            spaceshipRB.AddTorque(rollTorqueVec, ForceMode.Acceleration);
        }

        // 4) Yaw (рыскание) — клавиши Q / E
        float yawInput = 0f;
        if (Input.GetKey(KeyCode.E)) yawInput = +1f;
        else if (Input.GetKey(KeyCode.Q)) yawInput = -1f;

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
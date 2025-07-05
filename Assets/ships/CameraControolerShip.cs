using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraControllerShip : MonoBehaviour
{
    [Header("Цель и смещение")]
    [Tooltip("Трансформ корабля, за которым нужно следить")]
    public Transform target;

    [Tooltip("Расстояние от корабля (по его заднему направлению)")]
    public float distance = 10f;

    [Tooltip("Вертикальная высота относительно корабля")]
    public float height = 5f;

    [Header("Плавность движения")]
    [Tooltip("Скорость сглаживания перемещения (чем больше — тем быстрее камера встанет в нужную точку)")]
    [Range(1f, 20f)]
    public float positionSmoothSpeed = 8f;

    [Tooltip("Скорость сглаживания поворота (чем больше — тем быстрее камера опустит крен)")]
    [Range(1f, 20f)]
    public float rotationSmoothSpeed = 10f;

    private void LateUpdate()
    {
        if (target == null) return;

        Quaternion desiredRotation = Quaternion.LookRotation(target.forward, Vector3.up);

        Vector3 offset = desiredRotation * new Vector3(0f, 0f, -distance);
        Vector3 desiredPosition = target.position + Vector3.up * height + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            positionSmoothSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            rotationSmoothSpeed * Time.deltaTime
        );
    }
}
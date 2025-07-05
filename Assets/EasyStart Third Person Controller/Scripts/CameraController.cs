using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public enum WhichPlayer { Player1, Player2 }

    [Header("Split-Screen Settings")]
    public WhichPlayer forPlayer = WhichPlayer.Player1;
    [Range(0.1f, 0.9f)]
    public float leftScreenWidth = 0.5f;

    [Header("Target Settings")]
    public Transform player;

    [Header("Follow Settings")]
    public Vector3 offset = new Vector3(0f, 3f, -6f);
    public float positionSmoothTime = 0.2f;
    private Vector3 _positionVelocity;

    [Header("Rotation Settings")]
    public float rotationSmoothTime = 5f;

    [Header("Collision Settings")]
    public LayerMask collisionLayers = ~0; // по умолчанию — все
    public float sphereCastRadius = 0.3f;
    public float minDistance = 1.0f;

    void Awake()
    {
        var cam = GetComponent<Camera>();
        if (forPlayer == WhichPlayer.Player1)
            cam.rect = new Rect(0f, 0f, leftScreenWidth, 1f);
        else
            cam.rect = new Rect(leftScreenWidth, 0f, 1f - leftScreenWidth, 1f);
    }

    void Start()
    {
        if (player == null)
        {
            string tag = forPlayer == WhichPlayer.Player1 ? "Player1" : "Player2";
            var go = GameObject.FindWithTag(tag);
            if (go != null)
                player = go.transform;
            else
                Debug.LogError($"[{name}] Не найден объект с тегом «{tag}»");
      
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 worldOffset = player.TransformDirection(offset);
        Vector3 desiredPos = player.position + worldOffset;

        Vector3 eyePosition = player.position + Vector3.up * (offset.y * 0.5f);

        RaycastHit hit;
        Vector3 dir = (desiredPos - eyePosition).normalized;
        float dist = (desiredPos - eyePosition).magnitude;

        if (Physics.SphereCast(eyePosition, sphereCastRadius, dir, out hit, dist, collisionLayers, QueryTriggerInteraction.Ignore))
        {
            float correctedDist = Mathf.Max(hit.distance - sphereCastRadius, minDistance);
            desiredPos = eyePosition + dir * correctedDist;
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPos,
            ref _positionVelocity,
            positionSmoothTime
        );

        Vector3 lookPoint = player.position + Vector3.up * (offset.y * 0.5f);
        Quaternion desiredRot = Quaternion.LookRotation(lookPoint - transform.position);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRot,
            rotationSmoothTime * Time.deltaTime
        );
    }
}

using System.Collections;
using UnityEngine;

public class MinDeth : MonoBehaviour
{
    public Vector3 teleportPosition = new Vector3(110.4f, -8.6f, -184.2f);

    private void OnTriggerEnter(Collider other)
    {
        Transform current = other.transform;

        while (current != null)
        {
            if (current.CompareTag("Player"))
            {
                Destroy(gameObject, 0.1f);
                HandlePlayerCollision(current);
                return;
            }
            current = current.parent;
        }
    }

    private void HandlePlayerCollision(Transform player)
    {
        Debug.Log($"Обнаружен игрок: {player.name}");

        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            StartCoroutine(TeleportShip(rb));
        }
        else
        {
            player.position = teleportPosition;
        }
    }

    private IEnumerator TeleportShip(Rigidbody rb)
    {
        bool wasKinematic = rb.isKinematic;
        rb.isKinematic = true;

        rb.position = teleportPosition;
        rb.rotation = Quaternion.identity;

        yield return null;

        rb.isKinematic = wasKinematic;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Debug.Log("Корабль телепортирован!");
    }
}
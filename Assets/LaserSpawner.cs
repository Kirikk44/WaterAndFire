using UnityEngine;

public class CylinderSpawner : MonoBehaviour
{
    public GameObject cylinderPrefab;
    public int count = 10;
    public float spacing = 2f;
    public Vector3 direction = Vector3.right;

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = transform.position + direction.normalized * spacing * i;
            Instantiate(cylinderPrefab, position, Quaternion.identity);
        }
    }
}
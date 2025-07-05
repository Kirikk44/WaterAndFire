using UnityEngine;

public class SplitScreenCameraSwitcher : MonoBehaviour
{
    [Header("Person 1")]
    public GameObject person1;
    public Camera person1_StartCamera;
    public Camera person1_SwitchToCamera;

    [Header("Person 2")]
    public GameObject person2;
    public Camera person2_StartCamera;
    public Camera person2_SwitchToCamera;

    private void Start()
    {
        if (person1_StartCamera != null) person1_StartCamera.enabled = true;
        if (person1_SwitchToCamera != null) person1_SwitchToCamera.enabled = false;

        if (person2_StartCamera != null) person2_StartCamera.enabled = true;
        if (person2_SwitchToCamera != null) person2_SwitchToCamera.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform current = other.transform;

        while (current != null)
        {
            if (current.gameObject == person1)
            {
                SwitchCamera(person1_StartCamera, person1_SwitchToCamera);
                return;
            }

            if (current.gameObject == person2)
            {
                SwitchCamera(person2_StartCamera, person2_SwitchToCamera);
                return;
            }

            current = current.parent;
        }

    }

    private void SwitchCamera(Camera from, Camera to)
    {
        if (from != null) from.enabled = false;
        if (to != null) to.enabled = true;
    }
}

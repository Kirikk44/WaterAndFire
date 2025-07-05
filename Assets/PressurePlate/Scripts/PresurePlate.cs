using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    private bool isActivated = false;
    private int objectsOnPlate = 0;
    private Animator animator;

    [Header("События")]
    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("Animator component missing on PressurePlate!");

        animator?.SetBool("isPressed", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PickUp"))
        {
            objectsOnPlate++;
            if (!isActivated)
                ActivatePlate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PickUp"))
        {
            objectsOnPlate = Mathf.Max(0, objectsOnPlate - 1);
            if (objectsOnPlate == 0 && isActivated)
                DeactivatePlate();
        }
    }

    private void ActivatePlate()
    {
        isActivated = true;
        animator?.SetBool("isPressed", true);
        Debug.Log("Plate ACTIVATED");
        OnActivated?.Invoke();
    }

    private void DeactivatePlate()
    {
        isActivated = false;
        animator?.SetBool("isPressed", false);
        Debug.Log("Plate DEACTIVATED");
        OnDeactivated?.Invoke();
    }
}

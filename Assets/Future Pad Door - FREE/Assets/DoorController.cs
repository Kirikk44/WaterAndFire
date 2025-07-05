using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class DoorController : MonoBehaviour
{
    public PressurePlate plate;

    public Animator doorAnimator;
    public string isOpenParam = "isOpen";

    private void Reset()
    {
        doorAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        plate.OnActivated.AddListener(OpenDoor);
        plate.OnDeactivated.AddListener(CloseDoor);
    }

    private void OnDisable()
    {
        plate.OnActivated.RemoveListener(OpenDoor);
        plate.OnDeactivated.RemoveListener(CloseDoor);
    }

    private void OpenDoor()
    {
        doorAnimator.SetBool(isOpenParam, true);
        Debug.Log("Door opened (plate activated).");
    }

    private void CloseDoor()
    {
        doorAnimator.SetBool(isOpenParam, false);
        Debug.Log("Door closed (plate deactivated).");
    }
}

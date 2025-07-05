using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    public enum PortalID { Portal1, Portal2 }
    public PortalID portalId;

    public PortalController controller;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.name} вошёл в {portalId}");
        controller.OnPlayerEntered(portalId, other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        controller.OnPlayerExited(portalId, other.gameObject);
    }
}

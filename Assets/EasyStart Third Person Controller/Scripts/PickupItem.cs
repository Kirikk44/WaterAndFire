using System;
using System.Diagnostics;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Transform handPoint;
    public float pickupDistance = 2f;  
    public KeyCode pickupKey = KeyCode.E;

    public Vector3 heldScale = new Vector3(0.09f, 0.09f, 0.09f);

    private GameObject currentItem; 
    private Vector3 originalScale; 

    void Update()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            if (currentItem == null)
                TryPickupItem();
            else
                DropItem();
        }
    }

    void TryPickupItem()
    {
        RaycastHit hit;
        UnityEngine.Debug.DrawRay(transform.position, transform.forward * pickupDistance, Color.red, 3f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupDistance))
        {

            if (hit.collider.CompareTag("PickUp"))
            {
                currentItem = hit.collider.gameObject;

                originalScale = currentItem.transform.localScale;

                currentItem.transform.SetParent(handPoint);
                currentItem.transform.localPosition = Vector3.zero;
                currentItem.transform.localRotation = Quaternion.identity;

                


                Rigidbody rb = currentItem.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.isKinematic = true;

                UnityEngine.Debug.LogError("Уменьшили масшта,!");
                currentItem.transform.localScale = heldScale;
            }
        }
    }

    void DropItem()
    {
        if (currentItem != null)
        {
            currentItem.transform.SetParent(null);

            currentItem.transform.localScale = originalScale;

            Rigidbody rb = currentItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.linearVelocity = Vector3.zero; 
                rb.angularVelocity = Vector3.zero;
            }

            currentItem = null;
        }
    }
}

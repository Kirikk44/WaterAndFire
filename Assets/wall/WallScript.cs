using System.Collections.Generic;
using UnityEngine;

public class SelectivePassage : MonoBehaviour
{
    public List<GameObject> allowedPlayers;
    public Vector3 teleport = new Vector3((float)100.633, (float)4, (float)1.39); // куда телепортировать (относительно направления стены)

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (!allowedPlayers.Contains(obj))
        {
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            other.transform.position = teleport; // куда телепортировать

            if (cc != null) cc.enabled = true;
            //obj.transform.position = teleport;
        }
    }
}

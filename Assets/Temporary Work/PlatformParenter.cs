using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformParenter : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            other.transform.parent = null;
    }

}

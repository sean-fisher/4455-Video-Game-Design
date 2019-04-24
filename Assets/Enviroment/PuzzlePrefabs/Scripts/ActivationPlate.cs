using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationPlate : MonoBehaviour
{
    public GameObject platform;
    public ActivateMovingPlatforms plate;

    public Material green;
    public Material red;

    void Start()
    {
        plate = platform.GetComponent<ActivateMovingPlatforms>();
    }

    void OnTriggerStay(Collider c)
    {
        plate.turnOn = true;
    }

    void OnTriggerExit(Collider c)
    {
        plate.turnOn = false;
    }

    private void Update()
    {
        if (plate.turnOn)
        {
            MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
            if (mesh != null)
                mesh.material = green;
        }
        else
        {
            MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
            if (mesh != null)
                mesh.material = red;
        }
    }
}

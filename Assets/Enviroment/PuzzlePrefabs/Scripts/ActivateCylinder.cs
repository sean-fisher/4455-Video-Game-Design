using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCylinder : MonoBehaviour
{
    public GameObject cylinder;
    public TriggerCylinder plate;

    public Material green;
    public Material red;

    void Start()
    {
        plate = cylinder.GetComponent<TriggerCylinder>();
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

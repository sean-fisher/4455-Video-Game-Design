using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTrigger : MonoBehaviour
{
	public GameObject door;
	public MovingDoor doorSwitch;

    public Material green;
    public Material red;

	void Start()
    {
        doorSwitch = door.GetComponent<MovingDoor>();
	}
	void OnTriggerEnter(Collider c)
    {
        doorSwitch._switch = true;
	}
	void OnTriggerStay(Collider c) {
		doorSwitch._switch = true;
	}
	void OnTriggerExit(Collider c) {
		doorSwitch._switch = false;
	}

    private void Update()
    {
        if (doorSwitch._switch)
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

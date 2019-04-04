using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCylinder : MonoBehaviour
{
    public GameObject cylinder;
	public TriggerCylinder plate;
	void Start() {
		plate = cylinder.GetComponent<TriggerCylinder>();

	}
	void OnTriggerStay(Collider c) {
		plate.turnOn = true;
	}
	void OnTriggerExit(Collider c) {
		plate.turnOn = false;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTrigger : MonoBehaviour
{
	public GameObject door;
	public MovingDoor doorSwitch;
	void Start() {
		doorSwitch = door.GetComponent<MovingDoor>();

	}
	void OnTriggerEnter(Collider c) {
		doorSwitch._switch = true;
	}
	void OnTriggerStay(Collider c) {
		doorSwitch._switch = true;
	}
	void OnTriggerExit(Collider c) {
		doorSwitch._switch = false;
	}
}

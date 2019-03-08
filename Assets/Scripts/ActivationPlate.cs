using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationPlate : MonoBehaviour
{
	public GameObject platform;
	public ActivateMovingPlatforms plate;
	void Start() {
		plate = platform.GetComponent<ActivateMovingPlatforms>();

	}
	void OnTriggerStay(Collider c) {
		plate.turnOn = true;
	}
	void OnTriggerExit(Collider c) {
		plate.turnOn = false;
	}
}

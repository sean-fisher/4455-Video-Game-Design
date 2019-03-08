using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDoor : MonoBehaviour
{
    public Transform platform;
	public Transform start;
	public Transform end;
	public enum STATE {Auto, Switch, Button};
	public STATE state;
	public bool _switch;
	//public GameObject button;
	//public GameObject player;


	public float speed;

	Vector3 direction;
	Transform destination;

	void Start() {
		SetDest(start);
		state = STATE.Switch;
		_switch = false;
	}

	void FixedUpdate() {
		if (state == STATE.Auto) {
			platform.GetComponent<Rigidbody>().MovePosition(platform.position + direction*speed*Time.deltaTime);
			if (Vector3.Distance(platform.position, destination.position) < speed * Time.deltaTime) {
				SetDest(destination == start ? end : start);
			}
		}
		if (state == STATE.Switch) {
			//platform.GetComponent<Rigidbody>().MovePosition(platform.position + direction*speed*Time.deltaTime);
			if (!_switch) platform.position = Vector3.MoveTowards(platform.position, start.position, speed * Time.deltaTime);
        	if (_switch) platform.position = Vector3.MoveTowards(platform.position, end.position, speed * Time.deltaTime);  
        }     
        if (state == STATE.Button) {
        	 // if (Vector3.Distance(player.transform.position,button.transform.position) < 5) {
        	 // 	_switch = !_switch;
        	 // }
             if (!_switch) platform.position = Vector3.MoveTowards(platform.position, start.position, speed * Time.deltaTime);
             if (_switch) platform.position = Vector3.MoveTowards(platform.position, end.position, speed * Time.deltaTime);
         }
	}

	void SetDest(Transform dest) {
		destination = dest;
		direction = (destination.position - platform.position).normalized;
	}
	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(start.position,platform.localScale);
		
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(end.position,platform.localScale);
	}
}

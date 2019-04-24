using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscreteMovingPlatform : MonoBehaviour
{
    public Transform platform;
    public Transform start;
    public Transform end;

    public Button logicObject;
    public float speed;

    void FixedUpdate()
    {
        bool turnOn = logicObject.getStatus();

        if (turnOn)
        {
            platform.GetComponent<Rigidbody>().MovePosition(platform.position + (end.position - platform.position).normalized * speed * Time.deltaTime);
            if (Vector3.Distance(platform.position, end.position) < speed * Time.deltaTime)
            {
                platform.position = end.position;
            }
        }
        else
        {
            platform.GetComponent<Rigidbody>().MovePosition(platform.position + (start.position - platform.position).normalized * speed * Time.deltaTime);
            if (Vector3.Distance(platform.position, start.position) < speed * Time.deltaTime)
            {
                platform.position = start.position;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(start.position, platform.localScale);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(end.position, platform.localScale);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform platform;
    public Transform start;
    public Transform end;

    public float speed;

    Vector3 direction;
    Transform destination;

    void Start()
    {
        SetDest(start);
    }

    void FixedUpdate()
    {
        platform.transform.position = platform.position + direction * speed * Time.deltaTime;
        platform.GetComponent<Rigidbody>().MovePosition(platform.position + direction * speed * Time.deltaTime);
        if (Vector3.Distance(platform.position, destination.position) < speed * Time.deltaTime)
        {
            SetDest(destination == start ? end : start);
        }
    }

    void SetDest(Transform dest)
    {
        destination = dest;
        direction = (destination.position - platform.position).normalized;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(start.position, platform.localScale);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(end.position, platform.localScale);
    }

}

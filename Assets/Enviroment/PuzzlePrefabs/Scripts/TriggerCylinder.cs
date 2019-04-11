using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCylinder : MonoBehaviour
{

    Rigidbody rb;
    Vector3 velocity;
    public bool turnOn;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocity = new Vector3(0, 100, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (turnOn)
        {
            Quaternion rotate = Quaternion.Euler(velocity * Time.deltaTime);
            rb.MoveRotation(rb.rotation * rotate);
        }
    }
}

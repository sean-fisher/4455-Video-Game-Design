using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 v;
    private Vector3 priorPos;

    // Start is called before the first frame update
    void Start()
    {
        v = Vector3.zero;
        priorPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        v = (transform.position - priorPos) / Time.deltaTime;
        priorPos = transform.position;
        //Debug.Log(v);
    }

    public Vector3 GetVelocity() { return v; }
}

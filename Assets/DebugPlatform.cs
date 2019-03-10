using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlatform : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + Vector3.right * Time.deltaTime;
    }
}

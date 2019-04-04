using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTranslater : MonoBehaviour
{
    public GameObject box;
    public Transform t1;
    public Transform t2;
    public float time;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        box.transform.position = t1.position;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= time)
        {
            Transform tHolder = t1;
            t1 = t2;
            t2 = tHolder;
            timer = 0;
        }

        timer += Time.deltaTime;
        box.transform.position = Vector3.Lerp(t1.position, t2.position, timer/time);
    }
}

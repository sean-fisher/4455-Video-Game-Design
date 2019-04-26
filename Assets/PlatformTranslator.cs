using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTranslator : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public float rideDuration;

    float timer;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = start.position;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.position = Vector3.Lerp(start.position, end.position, (timer / rideDuration));
        if (Vector3.Distance(transform.position, end.position) < .1f)
        {
            transform.position = end.position;
            Transform holder = end;
            end = start;
            start = holder;
            timer = 0;
        }
    }
}

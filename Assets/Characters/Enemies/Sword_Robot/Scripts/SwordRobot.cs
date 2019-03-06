using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordRobot : MonoBehaviour
{
    Animator anim;
    public Transform target;
    float timeLim;
    float time;

    float hor;
    float ver;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        time = 0;
        timeLim = 0;
        hor = 0;
        ver = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > timeLim)
        {
            ChangeBehaviour();
        }

        ExecuteBehaviour();
    }

    void ChangeBehaviour()
    {
        hor = Random.Range(-1f, 1f);
        if (hor < -.5f)
            hor = -1;
        else if (hor > .5f)
            hor = 1;
        else
            hor = 0;

        ver = Random.Range(-1f, 1f);
        if (ver < -.5f)
            ver = -1;
        else if (ver > .5f)
            ver = 1;
        else
            ver = 0;

        time = 0;
        timeLim = Random.Range(0, 8);
    }

    void ExecuteBehaviour()
    {
        float priorH = anim.GetFloat("horizontal");
        float newH = Mathf.Lerp(priorH, hor, Time.deltaTime * .7f);
        anim.SetFloat("horizontal", newH);

        float priorV = anim.GetFloat("vertical");
        float newV = Mathf.Lerp(priorV, ver, Time.deltaTime * .7f);
        anim.SetFloat("vertical", newV);

        Quaternion currentHeading = transform.rotation;
        Quaternion desiredHeading = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(currentHeading, desiredHeading, Time.deltaTime * .2f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordRobot : MonoBehaviour
{
    Animator anim;
    public Transform target;
    public Collider[] hitBoxes;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        foreach (Collider hb in hitBoxes)
        {
            hb.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(target.transform.position - transform.position, Vector3.up), Vector3.up);
        float xzDis = (Vector3.ProjectOnPlane(target.transform.position,Vector3.up) - Vector3.ProjectOnPlane(transform.position, Vector3.up)).magnitude;
        float yDis = Mathf.Abs(target.transform.position.y - transform.position.y);
        if (xzDis <= 1.5f && yDis<= .5f)
        {
            anim.SetTrigger("attack");
        }
        else if (yDis > .5f)
        {
            anim.SetBool("running", false);
        }
        else
        {
            anim.SetBool("running", true);
        }
        //anim.SetTrigger("attack");
    }

    public void openHitboxes()
    {
        foreach (Collider hb in hitBoxes)
        {
            hb.enabled = true;
        }
    }

    public void closeHitboxes()
    {
        foreach (Collider hb in hitBoxes)
        {
            hb.enabled = false;
        }
    }
}

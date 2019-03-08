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
        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
        anim.SetTrigger("attack");
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

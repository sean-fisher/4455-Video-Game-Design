using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class RootMotionHook : MonoBehaviour
    {
        private Animator anim;
        private Rigidbody rb;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
    }

    private void OnAnimatorMove()
        {
            if (anim.applyRootMotion)
            {
                rb.drag = 0;
                Vector3 v = new Vector3(anim.deltaPosition.x, anim.deltaPosition.y, anim.deltaPosition.z);
                rb.velocity = v / Time.deltaTime;
            }
        }
    }
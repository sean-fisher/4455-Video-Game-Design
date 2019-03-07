using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class RootMotionHookStandard : MonoBehaviour
    {
        private Animator anim;
        private Rigidbody rb;
        private Vector3 groundNormal;

        private Vector3 velocity;

        private void Start()
        {
            anim = GetComponent<Animator>();
            anim.applyRootMotion = true;
            rb = GetComponentInParent<Rigidbody>();
            groundNormal = Vector3.up;
        }

        private void OnAnimatorMove()
        {
            if (anim.applyRootMotion)
            {
                Vector3 v = new Vector3(anim.deltaPosition.x, anim.deltaPosition.y, anim.deltaPosition.z) / Time.deltaTime;
                Vector3 dir;
                dir = Vector3.ProjectOnPlane(v, groundNormal).normalized;

                velocity = v.magnitude * dir;
            }
        }

        private void FixedUpdate()
        {
            if (anim.applyRootMotion)
                rb.velocity = velocity;
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class RootMotionHook : MonoBehaviour
    {
        private Animator anim;
        private Rigidbody rb;
        private Vector3 groundNormal;

        private void Start()
        {
            anim = GetComponent<Animator>();
            rb = GetComponentInParent<Rigidbody>();
            groundNormal = GetComponentInParent<Protag>().getGroundNormal();
        }

        private void OnAnimatorMove()
        {
            groundNormal = GetComponentInParent<Protag>().getGroundNormal();

            if (anim.applyRootMotion)
            {
                rb.drag = 0;
                Vector3 v = new Vector3(anim.deltaPosition.x, anim.deltaPosition.y, anim.deltaPosition.z) / Time.deltaTime;
                Vector3 dir = Vector3.ProjectOnPlane(v, groundNormal).normalized;
                Vector3 Velocity = v.magnitude * dir;
                rb.velocity = Velocity;
            }
        }
    }
}
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

        private Vector3 velocity;

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
                Vector3 v = new Vector3(anim.deltaPosition.x, anim.deltaPosition.y, anim.deltaPosition.z) / Time.deltaTime;
                Vector3 dir = Vector3.ProjectOnPlane(v, groundNormal).normalized;
                velocity = v.magnitude * dir;
            }
        }

        private void FixedUpdate()
        {
            if (anim.applyRootMotion)
                rb.velocity = velocity;
            //rb.AddForce(velocity * 10, ForceMode.Acceleration);
        }
    }
}
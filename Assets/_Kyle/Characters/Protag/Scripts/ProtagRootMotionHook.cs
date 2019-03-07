using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagRootMotionHook : MonoBehaviour
    {
        private Animator anim;
        private Rigidbody rb;
        private Vector3 groundNormal;
        private Vector3 wallNormal;

        private Vector3 velocity;
        private bool grounded;
        private bool climbing;

        private void Start()
        {
            anim = GetComponent<Animator>();
            rb = GetComponentInParent<Rigidbody>();
            groundNormal = GetComponentInParent<Protag>().getGroundNormal();
            wallNormal = Vector3.zero;
        }
        
        private void OnAnimatorMove()
        {
            Protag p = GetComponentInParent<Protag>();
            groundNormal = p.getGroundNormal();
            wallNormal = p.getClimableWallNormal();
            climbing = p.getClimbing();
            grounded = p.getGrounded();

            if (anim.applyRootMotion)
            {
                Vector3 v = new Vector3(anim.deltaPosition.x, anim.deltaPosition.y, anim.deltaPosition.z) / Time.deltaTime;
                Vector3 dir = v.normalized;
                if (grounded)
                    dir = Vector3.ProjectOnPlane(v, groundNormal).normalized;
                else if (climbing)
                     dir = Vector3.ProjectOnPlane(v, wallNormal).normalized;
                else
                    dir = Vector3.up;

                velocity = v.magnitude * dir;

                if (grounded)
                {
                    velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -20, 0), velocity.z);
                }
            }
        }

        private void FixedUpdate()
        {
            if (anim.applyRootMotion)
            {
                rb.velocity = velocity;
            }

                
            //rb.AddForce(velocity * 10, ForceMode.Acceleration);
        }

    }
}
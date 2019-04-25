using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagRootMotionHook : MonoBehaviour
    {
        private Protag p;
        private Animator anim;
        private Rigidbody rb;
        private Vector3 groundNormal;
        private Vector3 wallNormal;

        private Vector3 velocity;
        private bool grounded;
        private bool climbing;
        private bool aerial;

        private void Start()
        {
            anim = GetComponent<Animator>();
            rb = GetComponentInParent<Rigidbody>();
            groundNormal = GetComponentInParent<Protag>().getGroundNormal();
            wallNormal = Vector3.zero;
        }

        private void OnAnimatorMove()
        {
            p = transform.parent.GetComponent<Protag>();
            groundNormal = p.getGroundNormal();
            wallNormal = p.getClimableWallNormal();
            climbing = p.getClimbing();
            grounded = p.getGrounded();
            aerial = p.getAerial();

            if (anim.applyRootMotion)
            {
                Vector3 v = new Vector3(anim.deltaPosition.x, anim.deltaPosition.y, anim.deltaPosition.z) / Time.deltaTime;
                Vector3 dir = v.normalized;

                if (grounded && !aerial)
                {
                    dir = Vector3.ProjectOnPlane(v, groundNormal).normalized;
                    velocity = Vector3.ProjectOnPlane(v, groundNormal).magnitude * dir;
                    velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -20, 0), velocity.z);
                }
                else if (climbing)
                {
                    dir = Vector3.ProjectOnPlane(v, wallNormal).normalized;
                    velocity = v.magnitude * dir;
                }
                else if (aerial)
                {
                    
                    dir = Vector3.ProjectOnPlane(v, Vector3.up).normalized;
                    Debug.Log(dir);
                    velocity = Vector3.ProjectOnPlane(v, Vector3.up).magnitude * dir;
                    velocity = new Vector3(velocity.x, Mathf.Clamp(v.y, -20, 20), velocity.z);
                    Debug.Log(velocity);
                } 
            }
        }

        private void FixedUpdate()
        {
            if (anim.applyRootMotion)
            {
                if (velocity != null)
                    rb.velocity = velocity;
            }
        }
    }
}
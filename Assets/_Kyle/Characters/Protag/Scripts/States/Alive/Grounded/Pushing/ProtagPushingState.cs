using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagPushingState : ProtagGroundedState
    {
        #region variables
        protected override float animationTurnStrength { get { return 5f; } }
        protected override float physicsTurnStrength { get { return .15f; } }
        private Vector3 objectBeingPushedNormal;
        #endregion

        public override void enter(ProtagInput input)
        {
            base.enter(input);
            protag.anim.SetBool("pushing", true);
            protag.setRootMotion(true);
            protag.col.radius *= 2.2f;
            GameObject.FindObjectOfType<PlayerCameraControllerQuat>().CenterCamera();
        }

        public override void exit(ProtagInput input)
        {
            base.exit(input);
            protag.col.radius /= 2.2f;
            protag.anim.SetBool("pushing", false);
        }

        public override void runAnimation(ProtagInput input)
        {

        }

        public override bool runLogic(ProtagInput input)
        {
            if (!protag.isMovingForward()) {
                protag.newState<ProtagLocomotionState>();
            } else {
                
                RaycastHit hit;
                protag.GetPushableObjHitInfo(out hit);

                // is there still something in front of us?
                if (hit.collider == null) {
                    // no; so we return to standard ground movement
                    protag.newState<ProtagLocomotionState>();
                } else {
                    objectBeingPushedNormal = hit.normal;
                    
                    // are we oriented vertically enough that the climbing up ledge animation would be appropriate?
                    if (Mathf.Abs(Vector3.Angle(protag.anim.transform.forward, -hit.normal)) < 15) {
                        Rigidbody rb = protag.rb;//hit.collider.GetComponent<Rigidbody>();
                        if (rb) {
                            Vector3 force = -hit.normal * protag.rb.mass * protag.pushStrength;
                            force = new Vector3(force.x, force.y + 1, force.z);
                            rb.AddForce(force);
                            protag.anim.transform.rotation = Quaternion.LookRotation(-hit.normal, Vector3.up);
                        } else {
                            
                            protag.newState<ProtagLocomotionState>();
                        }
                    } else {
                            
                        protag.newState<ProtagLocomotionState>();
                    }


                }
            }
            return false;
        }

    }
}

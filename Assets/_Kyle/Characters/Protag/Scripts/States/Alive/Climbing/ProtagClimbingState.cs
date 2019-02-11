using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagClimbingState : ProtagAliveState
    {
        #region variables
        private bool jumpPressed;
        float timer;
        #endregion
        
        public override void enter(ProtagInput input)
        {
            protag.setRootMotion(true);
            protag.anim.SetBool("grounded", false);
            protag.anim.SetBool("climbing", true);
            protag.anim.SetBool("fall", false);
            jumpPressed = false;
            protag.rb.useGravity = false;
            protag.climbing = true;
        }

        public override void exit(ProtagInput input)
        {
            protag.anim.SetBool("climbing", false);
            protag.rb.useGravity = true;
            protag.rb.velocity = Vector3.zero;
            protag.climbing = false;
        }

        public override void runAnimation(ProtagInput input)
        {
            base.runAnimation(input);

            if (input.jump)
                jumpPressed = true;

            float dt = Time.deltaTime * 60f;
            timer += Time.deltaTime;

            float v = input.v;
            float h = input.h;
            float mag = input.totalMotionMag;
            
            Vector2 normalizedInput = new Vector2(h, v).normalized;
            PointNormalActionTypeTuple pnp = protag.checkClimbingWall();
            Vector3 wallNormal = pnp.normal;
            Vector3 wallTargetPos = pnp.point;

            if (wallNormal == Vector3.zero) {
                //Debug.Log("Wall normal is zero... maybe an issue?");
                return;
            }
            if (wallTargetPos == null) {
                Debug.LogError("Target pos is zero");
                return;
            }

            protag.setClimbableWallNormal(wallNormal);
            
            if (mag > 0)
            {
                // "up" relative to the wall and the player's orientation
                Vector3 dirToMoveVertical = Vector3.ProjectOnPlane(protag.modelTransform.up, wallNormal);

                // "right" relative to the wall and the player's orientation
                Vector3 dirToMoveHorizontal = Vector3.ProjectOnPlane(protag.modelTransform.right, wallNormal);

                Debug.DrawRay(transform.position, dirToMoveVertical * 2, Color.green);
                Debug.DrawRay(transform.position, dirToMoveHorizontal * 2, Color.red);
                Quaternion goalRot = Quaternion.LookRotation(-wallNormal, dirToMoveVertical);

                protag.modelTransform.rotation = Quaternion.Slerp(protag.modelTransform.rotation, goalRot, dt * .05f);
                Vector3 angles = protag.modelTransform.rotation.eulerAngles;
                protag.modelTransform.rotation = Quaternion.Euler(angles.x, angles.y, 0);

                // this code moves the player on the vertical axis. It should be handled by the root motion animations, but for some reason those aren't working.
                Vector3 yVec = dirToMoveVertical * Time.deltaTime * normalizedInput.y * 1;
                transform.position = transform.position + yVec * protag.yVecSpeed;
            }

            //set upwards animation/root motion
            float scale = (Mathf.Abs(protag.anim.GetFloat("vertical")) < Mathf.Abs(mag)) ? 1f : 4f; // speed up gradually, slow down quickly
            float nextV = Mathf.Lerp(protag.anim.GetFloat("vertical"), v, dt * .05f * scale);
            protag.anim.SetFloat("vertical", nextV);

            //Add horizontal animation/root motion
            scale = (Mathf.Abs(protag.anim.GetFloat("horizontal")) < Mathf.Abs(mag)) ? 1f : 4f; // speed up gradually, slow down quickly
            float nextH = Mathf.Lerp(protag.anim.GetFloat("horizontal"), h, dt * .05f * scale);
            protag.anim.SetFloat("horizontal", nextH);

            protag.anim.SetFloat("movementMagnitude", mag);

            if (wallTargetPos != null) {
                // move to the average point of the raycasthits. Currently will cause the player to slide
                //transform.position = Vector3.Lerp(transform.position, wallTargetPos, Time.deltaTime * 1);

                // force the player towards the wall
                protag.rb.AddForce(protag.modelTransform.forward * Time.deltaTime * 700);
                
                // since he's climbing a curved surface, this could cause some sliding.
                // So let's stop his movement if it seems he should be still.
                if (protag.rb.velocity.magnitude < .5f) {
                    protag.rb.velocity = Vector3.zero;
                }
            } else {
                // there's no wall in front of us.
                // this check probably shouldn't be in this function but it'll help test at the least
                protag.newState<ProtagFallingState>();
            }
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            PointNormalActionTypeTuple wallInfoTuple = protag.checkClimbingWall();
            Vector3 wallNormal = wallInfoTuple.normal;

            switch (wallInfoTuple.actionType) {
                case (ClimbingContextualActionType.CLIMBING):
                    if (jumpPressed)
                    {
                        protag.newState<ProtagFallingState>();
                        return true;
                    }
                    else if (Mathf.Abs(Vector3.Angle(wallNormal, Vector3.up)) <= 15)
                    {
                        // we've smoothly climbed onto a surface level enough to stand on
                        protag.newState<ProtagLocomotionState>();
                        return true;
                    }
                break;
                case (ClimbingContextualActionType.CLIMBUP):
                    Debug.Log("Climb up ledge!");
                    protag.newState<ProtagClimbingUpLedgeState>();
                break;
                case (ClimbingContextualActionType.CLIMBDOWN):
                break;
            }

            return false;
        }
    }
}
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
        }

        public override void exit(ProtagInput input)
        {
            protag.anim.SetBool("climbing", false);
            protag.rb.useGravity = true;
            protag.rb.velocity = Vector3.zero;
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
            Vector3 wallNormal = protag.checkClimbingWall();

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

                transform.position += dirToMoveVertical * Time.deltaTime * normalizedInput.y * 2;
            }

            //set upwards motion
            float scale = (Mathf.Abs(protag.anim.GetFloat("vertical")) < Mathf.Abs(mag)) ? 1f : 4f; // speed up gradually, slow down quickly
            float nextV = Mathf.Lerp(protag.anim.GetFloat("vertical"), v, dt * .05f * scale);
            protag.anim.SetFloat("vertical", nextV);

            //Add horizontal motion
            scale = (Mathf.Abs(protag.anim.GetFloat("horizontal")) < Mathf.Abs(mag)) ? 1f : 4f; // speed up gradually, slow down quickly
            float nextH = Mathf.Lerp(protag.anim.GetFloat("horizontal"), h, dt * .05f * scale);
            protag.anim.SetFloat("horizontal", nextH);

            protag.anim.SetFloat("movementMagnitude", mag);
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            Vector3 wallNormal = protag.checkClimbingWall();

            if (jumpPressed)
            {
                protag.newState<ProtagFallingState>();
                return true;
            }
            else if (Vector3.Angle(wallNormal, Vector3.up) <= 15)
            {
                protag.newState<ProtagLocomotionState>();
                return true;
            }

            return false;
        }
    }
}
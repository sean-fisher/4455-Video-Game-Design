using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public abstract class ProtagClimbingState : ProtagAliveState
    {
        #region variables
        protected abstract float climbingAnimationTurnStrength { get; }
        protected abstract float climbingPhysicsTurnStrength { get; }
        private bool jumpPressed;
        float timer;
        #endregion
        
        public override void enter(ProtagInput input)
        {
            protag.setRootMotion(true);
            protag.anim.SetBool("grounded", false);
            protag.anim.SetBool("climbing", true);
            protag.anim.SetBool("fall", false);
            protag.rb.useGravity = false;
        }

        public override void exit(ProtagInput input)
        {
            protag.anim.SetBool("climbing", false);
            protag.rb.useGravity = false;
            protag.rb.velocity = Vector3.zero;
        }

        public override void runAnimation(ProtagInput input)
        {
            base.runAnimation(input);

            float dt = Time.deltaTime * 60f;
            timer += Time.deltaTime;

            float v = input.v;
            float h = input.h;
            freeMovementAimation(v, h, input.totalMotionMag, dt);
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            //protag.rb.AddForce(-Vector3.ProjectOnPlane(Physics.gravity, protag.getGroundNormal()));

            return false;
        }

        private Vector3 lastRoot;
        private void freeMovementAimation(float v, float h, float mag, float dt)
        {
            if (lastRoot == null) {
                lastRoot = protag.anim.rootPosition;
            }
            Vector3 move = InputManager.calculateMove(v, h);
            Vector2 normalizedInput = new Vector2(h, v).normalized;
            Vector3 wallNormal = protag.checkClimbingWallNormal();

            if (true && move != Vector3.zero)
            {
                // "up" relative to the wall and the player's orientation
                Vector3 dirToMoveVertical = Vector3.Cross(-wallNormal, protag.modelTransform.right);

                // "right" relative to the wall and the player's orientation
                Vector3 dirToMoveHorizontal = Vector3.Cross(wallNormal, protag.modelTransform.up);

                Debug.DrawRay(transform.position, dirToMoveVertical * 3, Color.blue);
                Debug.DrawRay(transform.position, dirToMoveHorizontal * 3, Color.blue);
                Quaternion goalRot = Quaternion.LookRotation(-wallNormal, dirToMoveVertical);
                
                protag.modelTransform.rotation = goalRot;
                
                float distFromWall = .4f;
                float climbSpeed = 2;
                /*transform.position = 
                    protag.hitPoint                                                             // the center of the points from raycasts around the player
                    - protag.modelTransform.up * protag.col.height / 2                          // offset so the center of the model goes to the hitPoint
                    + protag.modelTransform.forward * -distFromWall                             // push the player away from wall
                  //  + dirToMoveVertical * Time.deltaTime * climbSpeed * normalizedInput.y       // move the character up/down the wall
                  //  + dirToMoveHorizontal * Time.deltaTime * climbSpeed * normalizedInput.x     // move the character left/right on the wall
                    ;// */
                transform.position += dirToMoveVertical * Time.deltaTime * normalizedInput.y * 2;
            }

            //set upwards motion
            float scale = (Mathf.Abs(protag.anim.GetFloat("vertical")) < Mathf.Abs(mag)) ? 1f : 4f; // speed up gradually, slow down quickly
            float targetV = mag;
            float nextV = Mathf.Lerp(protag.anim.GetFloat("vertical"),  normalizedInput.y, dt * .05f * scale);
            if (Mathf.Abs(nextV) - .01f < 0) nextV = 0;
            protag.anim.SetFloat("vertical", nextV);

            //Add horizontal motion
            scale = (Mathf.Abs(protag.anim.GetFloat("horizontal")) < Mathf.Abs(mag)) ? 1f : 4f; // speed up gradually, slow down quickly
            float nextH = Mathf.Lerp(protag.anim.GetFloat("horizontal"),  normalizedInput.x, dt * .05f * scale);
            if (Mathf.Abs(nextH) - .01f < 0) nextH = 0;
            protag.anim.SetFloat("horizontal", nextH);

            protag.anim.SetFloat("movementMagnitude", mag);

        }
    }
}
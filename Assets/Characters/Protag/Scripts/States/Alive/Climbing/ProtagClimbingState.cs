﻿using System.Collections;
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
            protag.climbingCol.enabled = true;
            protag.col.enabled = false;
            protag.col.radius = .11f;
        }

        public override void exit(ProtagInput input)
        {
            protag.anim.SetBool("climbing", false);
            protag.rb.useGravity = true;
            protag.rb.velocity = Vector3.zero;
            protag.climbing = false;
            protag.climbingCol.enabled = true;
            protag.col.enabled = true;

            protag.col.radius = .3f;
        }

        public override void runAnimation(ProtagInput input)
        {
            base.runAnimation(input);
            protag.checkClimbingWall();

            if (input.jump)
                jumpPressed = true;

            float dt = Time.deltaTime * 60f;

            timer += Time.deltaTime;

            float v = input.v;
            float h = input.h;
            float mag = input.totalMotionMag;
            
            Vector2 normalizedInput = new Vector2(h, v).normalized;
            Vector3 wallNormal = protag.getClimableWallNormal();
            Vector3 wallTargetPos = protag.getWallAnchorPosition();

            if (wallNormal == Vector3.zero) {
                //Debug.Log("Wall normal is zero... maybe an issue?");
                return;
            }
            if (wallTargetPos == Vector3.zero) {
                Debug.LogError("Target pos is zero");
                return;
            }
            
            if (mag > 0)
            {
                timer += Time.deltaTime;
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

                if (timer > 1f) {
                    SoundManager.Instance.PlaySoundFromGroupAtRandom("Grunt");
                    timer = 0;
                }
            } else {
                timer = 0;
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

            if (wallTargetPos != Vector3.zero) {
                // move to the average point of the raycasthits. Currently will cause the player to slide
                //transform.position = Vector3.Lerp(transform.position, wallTargetPos, Time.deltaTime * 1);

                if (v != 0 || h != 0) {
                    // force the character towards the wall, but only while the player is inputting movement.
                    // Otherwise the character would slide along the climbing surface
                    protag.rb.AddForce(protag.modelTransform.forward * Time.deltaTime * 3000);
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
            protag.checkClimbingWall();


            Vector3 wallNormal = protag.getClimableWallNormal();

            switch (protag.GetNextActionType()) {
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
                case (ClimbingContextualActionType.FALLOFF):
        
                protag.newState<ProtagFallingState>();
                return true;
                case (ClimbingContextualActionType.STANDUP):
        
                //protag.anim.SetTrigger("standUp");
                //protag.newState<ProtagLocomotionState>();
                    protag.newState<ProtagClimbingUpLedgeState>();
                return true;
            }

            return false;
        }
    }
}
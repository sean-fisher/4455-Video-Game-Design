using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public abstract class ProtagGroundedState : ProtagAliveState
    {
        #region v
        protected abstract float animationTurnStrength { get; }
        protected abstract float physicsTurnStrength { get; }
        private bool jumpPressed;
        #endregion

        public override void enter(ProtagInput input)
        {
            base.enter(input);
            protag.anim.SetBool("grounded", true);
            jumpPressed = false;
            protag.setRootMotion(true);
        }

        public override void exit(ProtagInput input)
        {
            base.exit(input);
            protag.anim.SetBool("grounded", false);
        }

        public override void runAnimation(ProtagInput input)
        {
            base.runAnimation(input);

            float dt = Time.deltaTime * 60f;
            float v = input.v;
            float h = input.h;

            if (input.jump)
                jumpPressed = true;

            freeMovementAimation(v, h, input.totalMotionMag, dt);
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            protag.checkGround();

            if (!protag.getGrounded())
            {
                protag.newState<ProtagFallingState>();
                return true;
            }
            else if (jumpPressed)
            {
                protag.newState<ProtagJumpingState>();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void freeMovementAimation(float v, float h, float mag, float dt)
        {
            // Calculate movement vector
            Vector3 ver = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized * v;
            Vector3 hor = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized * h;

            Vector3 move = hor + ver;
            move = Vector3.ClampMagnitude(move, 1);

            //rotate game object
            if (move != Vector3.zero)
            {
                Quaternion goalRot = Quaternion.LookRotation(move, Vector3.up);
                protag.transform.rotation = Quaternion.Slerp(protag.transform.localRotation, goalRot, physicsTurnStrength * dt * move.magnitude);
            }

            //set forward motion
            float scale;
            if (Mathf.Abs(protag.anim.GetFloat("vertical")) < Mathf.Abs(mag)) // speed up gradually
                scale = 1f;
            else                                                                       // slow down quickly
                scale = 4f;

            float targetV = mag;
            float nextV = Mathf.Lerp(protag.anim.GetFloat("vertical"), targetV, dt * .05f * scale);
            protag.anim.SetFloat("vertical", nextV);

            //set turn
            float turnAmount = mag * Vector3.Angle(protag.transform.forward, move) / 180;
            float turnDir = Utility.AngleDir(protag.transform.forward, move, Vector3.up);
            float targetH = turnDir * turnAmount * animationTurnStrength;
            float nextH = Mathf.Lerp(protag.anim.GetFloat("horizontal"), targetH, dt * .1f);
            protag.anim.SetFloat("horizontal", nextH);

            protag.anim.SetFloat("movementMagnitude", mag);
        }
    }
}

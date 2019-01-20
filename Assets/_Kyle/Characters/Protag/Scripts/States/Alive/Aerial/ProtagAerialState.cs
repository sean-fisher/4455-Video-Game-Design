using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public abstract class ProtagAerialState : ProtagAliveState
    {
        #region v
        protected abstract float aerialAnimationTurnStrength { get; }
        protected abstract float aerialPhysicsTurnStrength { get; }
        #endregion

        public override void enter(ProtagInput input)
        {
            protag.setRootMotion(false);
            protag.anim.SetBool("grounded", false);
        }

        public override void exit(ProtagInput input)
        {
            protag.anim.SetBool("grounded", true);
        }

        public override void runAnimation(ProtagInput input)
        {
            base.runAnimation(input);

            float dt = Time.deltaTime * 60f;
            float v = input.v;
            float h = input.h;

            freeMovementAimation(v, h, input.totalMotionMag, dt);
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            protag.checkGround();
            return false;
        }

        private void freeMovementAimation(float v, float h, float mag, float dt)
        {

            Vector3 ver = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized * v;
            Vector3 hor = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized * h;

            Vector3 move = hor + ver;
            move = Vector3.ProjectOnPlane(move, Vector3.up);
            move = Vector3.ClampMagnitude(move, 1);

            //Apply Aerial Force
            protag.rb.AddForce(move * protag.aerialMovementStrength * dt * 10, ForceMode.Force);
            protag.rb.velocity = new Vector3(protag.rb.velocity.x * protag.aerialDrag, protag.rb.velocity.y, protag.rb.velocity.z * protag.aerialDrag);

            if (move != Vector3.zero)
            {
                Quaternion goalRot = Quaternion.LookRotation(move, Vector3.up);
                protag.transform.rotation = Quaternion.Slerp(protag.transform.localRotation, goalRot, aerialPhysicsTurnStrength * dt * move.magnitude);
            }

            //set forward motion
            float targetV = mag;
            float nextV = Mathf.Lerp(protag.anim.GetFloat("vertical"), targetV, dt * .05f);
            protag.anim.SetFloat("vertical", nextV);

            //Add turn
            float turnAmount = mag * Vector3.Angle(protag.transform.forward, move) / 180;
            float turnDir = Utility.AngleDir(protag.transform.forward, move, Vector3.up);
            protag.anim.SetFloat("horizontal", turnDir * turnAmount * dt * aerialAnimationTurnStrength);

            protag.anim.SetFloat("movementMagnitude", mag);
        }
    }
}

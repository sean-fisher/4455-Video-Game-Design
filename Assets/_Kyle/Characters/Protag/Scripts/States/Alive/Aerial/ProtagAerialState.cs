using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public abstract class ProtagAerialState : ProtagAliveState
    {
        #region variables
        protected abstract float aerialAnimationTurnStrength { get; }
        protected abstract float aerialPhysicsTurnStrength { get; }
        float timer;
        #endregion

        public override void enter(ProtagInput input)
        {
            protag.setRootMotion(false);
            protag.setAerial(true);
            protag.anim.SetBool("grounded", false);
            timer = 0;
        }

        public override void exit(ProtagInput input)
        {
            protag.anim.SetBool("grounded", true);
            protag.setAerial(false);
        }

        public override void runAnimation(ProtagInput input)
        {
            base.runAnimation(input);

            float dt = Time.deltaTime * 60f;
            timer += Time.deltaTime;

            float v = input.v;
            float h = input.h;
            float mag = input.totalMotionMag;

            Vector3 move = InputManager.calculateMove(v, h);

            // rotates the player to the movement direction
            if (move != Vector3.zero)
            {
                Quaternion goalRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(protag.rb.velocity.normalized, Vector3.up), Vector3.up);
                protag.anim.transform.rotation = Quaternion.Slerp(protag.anim.transform.localRotation, goalRot, aerialPhysicsTurnStrength * dt * move.magnitude);
            }
            else
            {
                Quaternion goalRot = Quaternion.LookRotation(protag.anim.transform.forward, Vector3.up);
                protag.anim.transform.rotation = Quaternion.Slerp(protag.anim.transform.localRotation, goalRot, aerialPhysicsTurnStrength * dt);
            }

            // set forward motion
            float targetV = mag;
            float nextV = Mathf.Lerp(protag.anim.GetFloat("vertical"), targetV, dt * .05f);
            protag.anim.SetFloat("vertical", nextV);
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            
            //Apply Aerial Force
            Vector3 move = InputManager.calculateMove(input.v, input.h);
            protag.rb.AddForce(move * protag.aerialMovementStrength * 10, ForceMode.Force);
            protag.rb.velocity = new Vector3(protag.rb.velocity.x * protag.aerialDrag, protag.rb.velocity.y, protag.rb.velocity.z * protag.aerialDrag);

            if (timer >= .2)
                protag.setAerial(true);

            protag.checkClimableWallInFront();
            //protag.checkGround();
            protag.lerpRotationToUpwards();
            
            if (!protag.getGrounded() && protag.getIsClimbableWallInFront() && protag.isMovingForward())
            {
                protag.newState<ProtagClimbingState>();
                return true;
            }

            return false;
        }
    }
}

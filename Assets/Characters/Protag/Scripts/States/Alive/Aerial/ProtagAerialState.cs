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
        protected abstract bool applyAerialForce { get; }
        float timer;
        bool newJump;
        #endregion

        public override void enter(ProtagInput input)
        {
            timer = 0;
            protag.col.height = 1.4f;
            newJump = false;
        }

        public override void exit(ProtagInput input)
        {
            protag.anim.SetBool("grounded", true);
            protag.col.height = 1.8f;
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
                Quaternion goalRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(move, Vector3.up).normalized, Vector3.up);
                protag.anim.transform.localRotation = Quaternion.Slerp(protag.anim.transform.rotation, goalRot, aerialPhysicsTurnStrength * dt * move.magnitude);
                // set forward motion
                float targetV = 0;
                float nextV = Mathf.Lerp(protag.anim.GetFloat("vertical"), targetV, dt * .05f);
                protag.anim.SetFloat("vertical", nextV);
            }
            else
            {
                Quaternion goalRot = Quaternion.LookRotation(protag.anim.transform.forward, Vector3.up);
                protag.anim.transform.localRotation = Quaternion.Slerp(protag.anim.transform.rotation, goalRot, aerialPhysicsTurnStrength * dt);
                // set forward motion
                float targetV = mag;
                float nextV = Mathf.Lerp(protag.anim.GetFloat("vertical"), targetV, dt * .05f);
                protag.anim.SetFloat("vertical", nextV);
            }
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            //Apply Aerial Force
            Vector3 move = InputManager.calculateMove(input.v, input.h);
            
            if (applyAerialForce)
            {
                protag.rb.AddForce(move * protag.aerialMovementStrength * 10, ForceMode.Force);
                Vector3 horMove = Vector3.ClampMagnitude(Vector3.ProjectOnPlane(protag.rb.velocity, Vector3.up), 2);
                protag.rb.velocity = new Vector3(horMove.x, protag.rb.velocity.y, horMove.z);
                protag.rb.velocity = new Vector3(protag.rb.velocity.x * protag.aerialDrag, protag.rb.velocity.y, protag.rb.velocity.z * protag.aerialDrag);
            }
            
            if (timer >= .2)
                protag.setAerial(true);

            protag.checkInFront();
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

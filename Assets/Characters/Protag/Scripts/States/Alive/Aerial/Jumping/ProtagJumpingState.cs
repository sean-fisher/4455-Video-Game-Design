using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagJumpingState : ProtagAerialState
    {
        #region variables
        protected override float aerialAnimationTurnStrength { get { return 10f; } }
        protected override float aerialPhysicsTurnStrength { get { return .05f; } }
        protected override bool applyAerialForce { get { return timer > .18f; } }
        private float timer;
        private float dir;
        private bool jumped;
        private bool jumpAgain;
        #endregion

        public override void enter(ProtagInput input)
        {
            base.enter(input);
            protag.setRootMotion(false);
            protag.setGrounded(true);
            timer = 0f;
            dir = 1;
            protag.anim.SetTrigger("jump");
            protag.anim.SetFloat("aerial direction", dir);
            jumped = false;
            jumpAgain = false;
        }

        public override void exit(ProtagInput input)
        {
            base.exit(input);
        }

        public override void runAnimation(ProtagInput input)
        {
            base.runAnimation(input);
            timer += Time.deltaTime;
            dir -= Time.deltaTime;
            protag.anim.SetFloat("aerial direction", dir);

            if (InputManager.getJump())
                jumpAgain = true;
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            if (timer > .18 && !jumped)
            {
                protag.setRootMotion(true);
                protag.setAerial(true);
                jumped = true;
            }

            if (timer > .2)
            {
                protag.setGrounded(false);
            }

            if (timer > .3 && protag.getGrounded())
            {
                protag.setAerial(false);
                protag.newState<ProtagLandingState>();
                return true;
            }

            if (timer > .3 && jumpAgain && protag.doubleJumpAvailable)
            {
                
                //protag.newState<ProtagJumpingState>();
                protag.doubleJumpAvailable = false;
                //return true;
                
            }

            if (timer > .4)
            {
                protag.newState<ProtagFallingState>();
                return true;
            }

            return false;
        }
    }
}
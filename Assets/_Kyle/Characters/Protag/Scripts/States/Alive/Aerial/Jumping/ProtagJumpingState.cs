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
        private float timer;
        private float dir;
        private bool jumped;
        #endregion

        public override void enter(ProtagInput input)
        {
            base.enter(input);
            timer = 0f;
            dir = 1;
            protag.anim.SetTrigger("jump");
            protag.anim.SetFloat("aerial direction", dir);
            jumped = false;
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
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            if (timer > .2 && !jumped)
            {
                protag.rb.AddForce(Vector3.up * protag.jumpStrength, ForceMode.Impulse);
                jumped = true;
            }

            if (timer > .5 && protag.getGrounded())
            {
                Debug.Log("landing");
                protag.newState<ProtagLandingState>();
                return true;
            }

            return false;
        }
    }
}
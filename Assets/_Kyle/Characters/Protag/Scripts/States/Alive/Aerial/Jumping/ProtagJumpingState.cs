using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagJumpingState : ProtagAerialState
    {
        protected override float aerialAnimationTurnStrength { get { return 10f; } }
        protected override float aerialPhysicsTurnStrength { get { return .05f; } }
        private float timer;
        private bool jumped;

        public override void enter(ProtagInput input)
        {
            base.enter(input);
            timer = 0f;
            protag.anim.SetTrigger("jump");
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
                protag.newState<ProtagLandingState>();
                return true;
            }
            else if (timer > 1)
            {
                protag.newState<ProtagFallingState>();
                return true;
            }

            return false;
        }
    }
}
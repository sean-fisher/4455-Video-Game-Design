using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagLandingState : ProtagGroundedState
    {
        protected override float animationTurnStrength { get { return 10f; } }
        protected override float physicsTurnStrength { get { return .15f; } }
        private float timer;

        public override void enter(ProtagInput input)
        {
            base.enter(input);
            protag.anim.SetTrigger("land");
            timer = 0;
        }

        public override void exit(ProtagInput input)
        {
            base.exit(input);
        }

        public override void runAnimation(ProtagInput input)
        {
            timer += Time.deltaTime;
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            if (timer > .5 || input.v > .1)
            {
                protag.newState<ProtagLocomotionState>();
                return true;
            }

            return false;
        }
    }
}

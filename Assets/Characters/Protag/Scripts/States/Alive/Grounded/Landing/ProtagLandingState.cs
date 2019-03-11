using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagLandingState : ProtagGroundedState
    {
        #region variables
        protected override float animationTurnStrength { get { return 10f; } }
        protected override float physicsTurnStrength { get { return .15f; } }
        private float timer;
        #endregion

        public override void enter(ProtagInput input)
        {
            base.enter(input);
            protag.anim.SetBool("land", true);
            timer = 0;
        }

        public override void exit(ProtagInput input)
        {
            base.exit(input);
            protag.anim.SetBool("land", false);
            protag.anim.SetFloat("compression", 0);
        }

        public override void runAnimation(ProtagInput input)
        {
            base.runAnimation(input);
            timer += Time.deltaTime;
            float sampleLocation = protag.sampleCompressionCurve(timer);
            protag.anim.SetFloat("compression", sampleLocation);
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            if (timer > .5)
            {
                protag.newState<ProtagLocomotionState>();
                return true;
            }

            return false;
        }
    }
}

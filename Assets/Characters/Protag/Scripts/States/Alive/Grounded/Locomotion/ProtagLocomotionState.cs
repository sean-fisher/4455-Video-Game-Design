using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagLocomotionState : ProtagGroundedState
    {
        #region variables
        protected override float animationTurnStrength { get { return 5f; } }
        protected override float physicsTurnStrength { get { return .15f; } }
        #endregion

        public override void enter(ProtagInput input)
        {
            base.enter(input);
        }

        public override void exit(ProtagInput input)
        {
            base.exit(input);
        }

        public override void runAnimation(ProtagInput input)
        {
            base.runAnimation(input);
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            if (Input.GetMouseButton(1) && input.totalMotionMag > .2f)
            {
                protag.newState<ProtagSprintingState>();
                return true;
            }

            return false;
        }
    }
}

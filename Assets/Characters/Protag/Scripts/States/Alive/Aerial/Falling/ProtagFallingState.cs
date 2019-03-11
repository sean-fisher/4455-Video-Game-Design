using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagFallingState : ProtagAerialState
    {
        #region variables
        protected override float aerialAnimationTurnStrength { get { return 10f; } }
        protected override float aerialPhysicsTurnStrength { get { return .05f; } }
        #endregion

        public override void enter(ProtagInput input)
        {
            base.enter(input);
            protag.anim.SetBool("fall", true);
            protag.anim.SetFloat("aerial direction", -1);
        }

        public override void exit(ProtagInput input)
        {
            base.exit(input);
            protag.anim.SetBool("fall", false);
        }

        public override void runAnimation(ProtagInput input)
        {
            base.runAnimation(input);
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            if (protag.getGrounded())
            {
                protag.newState<ProtagLandingState>();
                return true;
            }
            return false;
        }
    }
}

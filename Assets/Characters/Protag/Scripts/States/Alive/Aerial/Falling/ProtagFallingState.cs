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
        protected override bool applyAerialForce { get { return true; } }
        bool jump;
        #endregion

        public override void enter(ProtagInput input)
        {
            base.enter(input);
            protag.setRootMotion(true);
            protag.setAerial(true);
            protag.anim.SetBool("grounded", false);
            protag.anim.SetBool("fall", true);
            protag.anim.SetFloat("aerial direction", -1);
            jump = false;
        }

        public override void exit(ProtagInput input)
        {
            base.exit(input);
            protag.anim.SetBool("fall", false);
        }

        public override void runAnimation(ProtagInput input)
        {
            base.runAnimation(input);

            if (InputManager.getJump())
                jump = true;
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            if (protag.getGrounded())
            {
                protag.setAerial(false);
                protag.newState<ProtagLandingState>();
                return true;
            }

            if (jump && protag.doubleJumpAvailable)
            {
                protag.newState<ProtagJumpingState>();
                protag.doubleJumpAvailable = false;
                return true;
            }
            return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagClimbingUpLedgeState : ProtagClimbingState
    {
        #region variables
        private float timer;
        #endregion

        public override void enter(ProtagInput input)
        {
            base.enter(input);
            protag.anim.SetBool("climbing", false);
            protag.setRootMotion(true);
            protag.climbing = true;
            protag.col.enabled = false;
            protag.rb.velocity = Vector3.zero;
            protag.rb.angularVelocity = Vector3.zero;
            timer = 0;
        }

        public override void exit(ProtagInput input)
        {
            base.exit(input);
            protag.anim.SetFloat("climbUp", 0);
            protag.col.enabled = true;
            protag.climbing = false;
        }

        public override void runAnimation(ProtagInput input)
        {
            //base.runAnimation(input);
            timer += Time.deltaTime;
            float sampleLocation = protag.sampleClimbUpCurve(timer);
            protag.anim.SetFloat("climbUp", sampleLocation);
        }

        public override bool runLogic(ProtagInput input)
        {
            //if (base.runLogic(input))
            //    return true;

            if (timer > 1)
            {
                protag.newState<ProtagLocomotionState>();
                return true;
            }

            return false;
        }
    }
}

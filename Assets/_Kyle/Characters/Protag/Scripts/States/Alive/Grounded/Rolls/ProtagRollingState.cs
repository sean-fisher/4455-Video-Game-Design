using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TCS.Characters
{
    public class ProtagRollingState : ProtagGroundedState
    {
        protected override float animationTurnStrength { get { return 1f; } }
        protected override float physicsTurnStrength { get { return .2f; } }

        float timer = 0;
        float earliestExitTime = .5f;

        public override void enter(ProtagInput input)
        {
            protag.anim.SetTrigger("roll");
            protag.setVulnerable(false);
        }

        public override void exit(ProtagInput input)
        {
            protag.setVulnerable(true);
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

            if (timer > earliestExitTime)
            {
                if (protag.anim.GetBool("Locomotion"))
                {
                    protag.newState<ProtagLocomotionState>();
                    return true;
                }
            }

            return false;
        }
    }
}
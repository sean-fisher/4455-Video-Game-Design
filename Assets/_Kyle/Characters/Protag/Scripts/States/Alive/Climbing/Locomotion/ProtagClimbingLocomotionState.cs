using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagClimbingLocomotionState : ProtagClimbingState
    {
        #region variables
        protected override float climbingAnimationTurnStrength { get { return 10f; } }
        protected override float climbingPhysicsTurnStrength { get { return .05f; } }
        private float timer;
        private float dir;
        private bool jumped;
        #endregion

        public override void enter(ProtagInput input)
        {
            base.enter(input);
            timer = 0f;
            dir = 1;
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
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            // TODO ? Don't know if editing this will be necessary -- Sean

            return false;
        }
    }
}
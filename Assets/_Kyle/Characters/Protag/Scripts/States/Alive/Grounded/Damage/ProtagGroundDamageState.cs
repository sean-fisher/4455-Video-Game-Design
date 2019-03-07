using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagGroundDamageState : ProtagGroundedState
    {
        private float timer;
        private float timeLimit = .67f;

        protected override float animationTurnStrength { get {return 0; }}
        protected override float physicsTurnStrength { get { return 0; } }

        public override void enter(ProtagInput input)
        {
            protag.isVulnerable = false;
            Vector3 dir = protag.anim.transform.InverseTransformDirection(input.dmg.dir);
            protag.anim.SetTrigger("groundDamage");
            protag.anim.SetFloat("horizontal", dir.x);
            protag.anim.SetFloat("vertical", dir.z);
            timer = 0;
        }

        public override void exit(ProtagInput input)
        {
            protag.isVulnerable = true;
            protag.anim.SetFloat("horizontal", input.h);
            protag.anim.SetFloat("vertical", input.v);
            protag.CleanDamage();
            base.exit(input);
        }

        public override void runAnimation(ProtagInput input)
        {
            timer += Time.deltaTime;
            float dt = Time.deltaTime * 60f;
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            protag.lerpRotationToUpwards();
            protag.rb.AddForce(-Vector3.ProjectOnPlane(Physics.gravity, protag.getGroundNormal()));

            if (timer >= timeLimit)
            {
                protag.newState<ProtagLocomotionState>();
                return true;
            }

            return false;
        }
    }
}

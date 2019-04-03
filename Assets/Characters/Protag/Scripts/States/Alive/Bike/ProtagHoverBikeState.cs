using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagHoverBikeState : ProtagAliveState
    {

        private float physicsTurnStrength = .1f;
        private float force = 10;

        public override void enter(ProtagInput input)
        {
            protag.anim.applyRootMotion = false;
        }

        public override void exit(ProtagInput input)
        {
            
        }

        public override void runAnimation(ProtagInput input)
        {
            base.runAnimation(input);
        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            base.runAnimation(input);

            float dt = Time.deltaTime * 60f;
            float v = input.v;
            float h = input.h;
            float mag = input.totalMotionMag;

            Vector3 move = InputManager.calculateMove(v, h).normalized;

            // rotation assistance for game object
            if (move != Vector3.zero)
            {
                Quaternion goalRot = Quaternion.LookRotation(move, Vector3.up);
                protag.anim.transform.rotation = Quaternion.Slerp(protag.anim.transform.localRotation, goalRot, physicsTurnStrength * dt * move.magnitude);
            }

            protag.rb.AddForce(move * force, ForceMode.Acceleration);
            float magnitude = Mathf.Clamp(protag.rb.velocity.magnitude, 0, 10);
            protag.rb.velocity = protag.rb.velocity.normalized * magnitude;

            if (move == Vector3.zero)
                protag.rb.velocity *= .95f;

            return false;
        }
    }
}

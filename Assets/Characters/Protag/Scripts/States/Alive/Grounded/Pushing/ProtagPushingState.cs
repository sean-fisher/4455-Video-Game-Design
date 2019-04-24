using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagPushingState : ProtagGroundedState
    {
        #region variables
        protected override float animationTurnStrength { get { return 5f; } }
        protected override float physicsTurnStrength { get { return .15f; } }
        private Vector3 objectBeingPushedNormal;
        #endregion

        public override void enter(ProtagInput input)
        {
            base.enter(input);
            protag.anim.SetBool("pushing", true);
            protag.setRootMotion(false);
            protag.col.radius = .75f;
            GameObject.FindObjectOfType<PlayerCameraController>().CenterCamera();
        }

        public override void exit(ProtagInput input)
        {
            base.exit(input);
            protag.col.radius = .3f;
            protag.anim.SetBool("pushing", false);
        }

        public override void runAnimation(ProtagInput input)
        {

        }

        public override bool runLogic(ProtagInput input)
        {
            if (base.runLogic(input))
                return true;

            RaycastHit hit;
            protag.GetPushableObjHitInfo(out hit);

            if (!protag.isMovingForward() || hit.collider == null)
            {
                protag.newState<ProtagLocomotionState>();
                return true;
            }

            if (Mathf.Abs(Vector3.Angle(protag.anim.transform.forward, -hit.normal)) < 15)
            {
                Vector3 desiredDir = new Vector3(-hit.normal.x, (int)-hit.normal.y, -hit.normal.z);
                protag.anim.transform.rotation = Quaternion.LookRotation(desiredDir, Vector3.up);
                Vector3 v = (hit.rigidbody.transform.position - protag.transform.position).normalized;
                
                protag.rb.AddForce(v * protag.pushStrength);
            }
            else
            {
                protag.newState<ProtagLocomotionState>();
                return true;
            }

            return false;
        }

    }
}

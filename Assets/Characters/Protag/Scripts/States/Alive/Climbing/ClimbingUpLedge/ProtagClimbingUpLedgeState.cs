using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagClimbingUpLedgeState : ProtagClimbingState
    {
        #region variables
        private float timer;
        private float clipLength = 1;
        
        // needed so we set the player right on the ground instead
        // of having an awkward short fall on occasion
        Vector3 targetPosition; 
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
            protag.movementEnabled = false; // prevent sliding while climbing up
            protag.anim.SetTrigger("climbUp");
            targetPosition = protag.getWallAnchorPosition();
            timer = 0;

            SoundManager.Instance.PlayAnySFX("ClimbUp");
        }

        public override void exit(ProtagInput input)
        {
            base.exit(input);
            protag.col.enabled = true;
            protag.climbing = false;
            protag.movementEnabled = true; // prevent sliding while climbing up
        }

        public override void runAnimation(ProtagInput input)
        {
            var stateInfo = protag.anim.GetCurrentAnimatorStateInfo(0);
            //clipLength = stateInfo.length;
            if  (clipLength > 3) {
                Debug.Log("Wrong clip length");
            }

            //base.runAnimation(input);
            timer += Time.deltaTime;
            float sampleLocation = protag.sampleClimbUpCurve(timer);
            protag.rb.velocity = Vector3.zero;
            
            Vector3 offset = targetPosition - protag.transform.position;

            // move the player toward the target position so we make sure our feet 
            // are directly on the ground by the end of the animation
            float progress = (timer / clipLength);
            protag.transform.position += offset * progress * .25f;
        }

        public override bool runLogic(ProtagInput input)
        {

            if (timer > clipLength)
            {
                protag.newState<ProtagLocomotionState>();
                return true;
            }

            return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class SwordRobotAnimationEventReciever : MonoBehaviour
    {
        public void OpenDamageColliders()
        {
            SwordRobot sr = this.GetComponentInParent<SwordRobot>();
            sr.openHitboxes();
        }

        public void CloseDamageColliders()
        {
            SwordRobot sr = this.GetComponentInParent<SwordRobot>();
            sr.closeHitboxes();
        }

        public void LeftFootstep()
        {
            if (InputManager.getTotalMotionMag() > .2f)
            {
                SwordRobot sr = this.GetComponentInParent<SwordRobot>();
                //sr.sfx.playLeftFootstep();
            }
        }

        public void RightFootstep()
        {
            if (InputManager.getTotalMotionMag() > .2f)
            {
                SwordRobot sr = this.GetComponentInParent<SwordRobot>();
                //sr.sfx.playRightFootstep();
            }
        }
    }


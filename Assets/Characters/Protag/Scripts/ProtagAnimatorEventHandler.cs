using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters {
    public class ProtagAnimatorEventHandler : MonoBehaviour
    {
        Protag protag;

        private void Start()
        {
            protag = GetComponentInParent<Protag>();
        }

        public void LeftFootstep()
        {
            protag.sfx.playLeftFootstep();
        }

        public void RightFootstep()
        {
            protag.sfx.playLeftFootstep();
        }

        public void RollSound()
        {
            protag.sfx.playRollSound();
        }

        public void DeathSound()
        {
            protag.sfx.playDeathSound();
        }
    }
}


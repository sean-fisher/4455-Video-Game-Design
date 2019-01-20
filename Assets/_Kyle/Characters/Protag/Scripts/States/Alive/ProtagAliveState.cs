using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public abstract class ProtagAliveState : ProtagState
    {
        public override void enter(ProtagInput input)
        {
            
        }

        public override void exit(ProtagInput input)
        {
            
        }

        public override void runAnimation(ProtagInput input)
        {
            float normalTime = protag.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            float length = protag.anim.GetCurrentAnimatorStateInfo(0).length;
            protag.anim.SetFloat("stateTime", (normalTime % length) / length);
        }

        public override bool runLogic(ProtagInput input)
        {
            return false;
        }
    }
}

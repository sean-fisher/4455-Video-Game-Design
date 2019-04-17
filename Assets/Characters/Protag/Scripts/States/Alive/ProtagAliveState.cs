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

        }

        public override bool runLogic(ProtagInput input)
        {
            if (protag.hp <= 0)
            {
                protag.newState<ProtagDeathState>();
                return true;
            }
            return false;
        }
    }
}

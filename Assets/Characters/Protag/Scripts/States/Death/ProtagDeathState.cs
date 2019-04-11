using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagDeathState : ProtagState
    {
        public override void enter(ProtagInput input)
        {
            protag.anim.SetTrigger("dead");
        }

        public override void exit(ProtagInput input)
        {

        }

        public override void runAnimation(ProtagInput input)
        {

        }

        public override bool runLogic(ProtagInput input)
        {
            return false;
        }
    }
}
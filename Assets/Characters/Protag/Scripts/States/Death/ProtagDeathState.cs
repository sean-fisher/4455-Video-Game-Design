using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TCS.Characters
{
    public class ProtagDeathState : ProtagState
    {

        float timer;
        Image blackFade;
        float fadeOutLength = 5;
        

        public override void enter(ProtagInput input)
        {
            protag.anim.SetTrigger("dead");
            Transitioner.Instance.LoadSceneWithFades(SceneManager.GetActiveScene().name, 3, 1, Color.red);

            SoundManager.Instance.PlayAnySFX("Death");
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
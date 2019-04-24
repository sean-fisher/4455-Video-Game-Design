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
            blackFade = GameObject.FindGameObjectWithTag("BlackFade").GetComponent<Image>();
            timer = 0;
        }

        public override void exit(ProtagInput input)
        {

        }

        public override void runAnimation(ProtagInput input)
        {
            timer += Time.deltaTime;

            if (blackFade != null)
                blackFade.color = new Color(blackFade.color.r, blackFade.color.g, blackFade.color.b, timer / fadeOutLength);

            if (timer >= fadeOutLength)
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.buildIndex);
            }
        }

        public override bool runLogic(ProtagInput input)
        {
            return false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class ProtagSFX : MonoBehaviour
    {
        [Header("Audio Sources")]
        public AudioSource leftFootSource;
        public AudioSource rightFootSource;
        public AudioSource voiceSource;
        public AudioSource rollSource;

        [Header("Footsteps")]
        public AudioClip[] footsteps;

        [Header("Character Voicelines")]
        public AudioClip deathClip;

        [Header("Roll Clip")]
        public AudioClip rollClip;

        public void playLeftFootstep()
        {
            if (!leftFootSource.isPlaying)
            {
                leftFootSource.clip = footsteps[Random.Range(0, footsteps.Length)];
                leftFootSource.Play();
            }

        }

        public void playRollSound()
        {
            if (!rollSource.isPlaying)
            {
                rollSource.clip = rollClip;
                rollSource.Play();
            }
        }

        public void playRightFootstep()
        {
            if (!rightFootSource.isPlaying)
            {
                rightFootSource.clip = footsteps[Random.Range(0, footsteps.Length)];
                rightFootSource.Play();
            }
        }

        public void playDeathSound()
        {
            if (deathClip == null)
                return;

            if (!voiceSource.isPlaying)
            {
                voiceSource.clip = deathClip;
                voiceSource.Play();
            }
        }
    }
}

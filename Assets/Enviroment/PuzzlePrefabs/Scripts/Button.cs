using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool toggle;
    public Material red;
    public Material green;

    public AudioClip onSound;
    public AudioClip offSound;

    private AudioSource audioSource;

    private bool status;

    private void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    public bool getStatus()
    {
        return status;
    }

    void OnTriggerEnter(Collider c)
    {
        status = !status;
        SetColor();
    }
    void OnTriggerExit(Collider c)
    {

    }

    private void SetColor()
    {

        Renderer rend = GetComponentInChildren<Renderer>();

        // change color of button
        if (status)
        {
            rend.material = green;
            audioSource.clip = onSound;
            audioSource.PlayOneShot(onSound);
        }
        else
        {
            rend.material = red;
            audioSource.clip = offSound;
            audioSource.PlayOneShot(offSound);
        }
    }

}

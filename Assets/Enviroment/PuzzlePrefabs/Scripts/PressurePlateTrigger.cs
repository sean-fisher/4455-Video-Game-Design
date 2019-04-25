using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTrigger : MonoBehaviour
{
    public GameObject door;
    public MovingDoor doorSwitch;

    public Material green;
    public Material red;

    public AudioClip onSound;
    public AudioClip offSound;

    private AudioSource audioSource;

    void Start()
    {
        doorSwitch = door.GetComponent<MovingDoor>();
        audioSource = GetComponentInChildren<AudioSource>();
    }
    void OnTriggerEnter(Collider c)
    {
        doorSwitch._switch = true;
        if (audioSource)
        {
            audioSource.clip = onSound;
            audioSource.PlayOneShot(onSound);
        }
        
    }
    void OnTriggerStay(Collider c)
    {
        doorSwitch._switch = true;
    }
    void OnTriggerExit(Collider c)
    {
        doorSwitch._switch = false;
        if (audioSource)
        {
            audioSource.clip = offSound;
            audioSource.PlayOneShot(offSound);
        }
    }

    private void Update()
    {
        if (doorSwitch._switch)
        {
            MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
            if (mesh != null)
                mesh.material = green;
        }
        else
        {
            MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
            if (mesh != null)
                mesh.material = red;
        }
    }
}

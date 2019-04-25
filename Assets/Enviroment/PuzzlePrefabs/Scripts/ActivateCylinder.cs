using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCylinder : MonoBehaviour
{
    public GameObject cylinder;
    public TriggerCylinder plate;

    public Material green;
    public Material red;

    public AudioClip onSound;
    public AudioClip offSound;

    private AudioSource audioSource;

    void Start()
    {
        plate = cylinder.GetComponent<TriggerCylinder>();
        audioSource = GetComponentInChildren<AudioSource>();
    }
    void OnTriggerStay(Collider c)
    {
        plate.turnOn = true;
        audioSource.clip = onSound;
        audioSource.PlayOneShot(onSound);
    }
    void OnTriggerExit(Collider c)
    {
        plate.turnOn = false;
        audioSource.clip = offSound;
        audioSource.PlayOneShot(offSound);
    }

    private void Update()
    {
        if (plate.turnOn)
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

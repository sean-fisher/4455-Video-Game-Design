using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool toggle;
    public Material red;
    public Material green;

    private bool status;

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
        //status = toggle ? status : false;
    }

    private void Update()
    {
        Debug.Log(status);
    }

    private void SetColor()
    {

        Renderer rend = GetComponentInChildren<Renderer>();

        // change color of button
        if (status)
        {
            rend.material = green;
        }
        else
        {
            rend.material = red;
        }
    }

}

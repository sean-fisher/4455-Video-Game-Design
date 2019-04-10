using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool toggle;

    private bool status;

    public bool getStatus()
    {
        return status;
    }

    void OnTriggerEnter(Collider c)
    {
        status = toggle ? !status : true;
        SetColor();
    }
    void OnTriggerExit(Collider c)
    {
        status = toggle ? status : false;
    }

    private void SetColor()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("_Color");
        // change color of button
        if (status)
        {
            rend.material.SetColor("_Color", Color.green);
        }
        else
        {
            rend.material.SetColor("_Color", Color.red);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Menu : MonoBehaviour
{
    public void QuitToDesktop() {
        
    }

    public virtual void Open(){}
    public virtual void Close(){}
}

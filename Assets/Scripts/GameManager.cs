using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public Canvas uiElements;
    public LevelManager levelManager;

    void Awake() {
        GameObject.Instantiate(uiElements);
        DontDestroyOnLoad(this);
        GameObject.Instantiate(levelManager);
    }
}

public static class RunOnStart
{

    [RuntimeInitializeOnLoadMethod]
    static void InitGame() {
        Debug.Log("Initialized Game");
        GameObject.Instantiate(Resources.Load("GameManager"));
    }
}

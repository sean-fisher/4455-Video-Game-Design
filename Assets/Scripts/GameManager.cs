using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public Canvas uiElements;
    public LevelManager levelManager;
    public SoundManager soundManager;

    void Awake() {
        DontDestroyOnLoad(this);

        GameObject.Instantiate(uiElements);
        GameObject.Instantiate(levelManager);
        GameObject.Instantiate(soundManager);
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

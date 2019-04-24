using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject pauseMenu;
    public LevelManager levelManager;

    void Awake() {
        GameObject.Instantiate(pauseMenu, GameObject.FindObjectOfType<Canvas>().transform);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : Menu
{
    static bool isPaused;
    public static bool IsPaused {
        get {
            return isPaused;
        }
    }
    static PauseMenu singleton;
    public GameObject holder;
    bool isOpen = false;


    void Awake() {
        if (singleton == null) {
            singleton = this;
            holder.SetActive(false);
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
    }

    void Update() {
        // this is bad/hacky way of making sure we aren't in the title screen or another place 
        // where you shouldn't be able to pause
        if (InputManager.getPause() && (GameObject.FindObjectOfType<TCS.Characters.Protag>().enabled == true)) {
            if (isOpen) {
                Close();
            } else {
                Open();
            }
        }
    }


    public override void Open() {
        holder.SetActive(true);
        Time.timeScale = 0.0001f;
        isPaused = true;
        isOpen = true;
        transform.GetChild(0).GetComponentInChildren<Menu>().EnterMenu();
    }
    public override void Close() {
        holder.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        isOpen = false;
    }

    public void ReturnToMainMenu() {
        Close();
        Transitioner.Instance.LoadSceneWithFades("Title");

    }

    public static PauseMenu Instance() {
        return singleton;
    }

    public void RestartLevel() {
        Transitioner.Instance.LoadSceneWithFades(SceneManager.GetActiveScene().name);
        Close();
    }
}

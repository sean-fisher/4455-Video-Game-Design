using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : Menu
{
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
        if (InputManager.getPause()) {
            if (isOpen) {
                Close();
            } else {
                Open();
            }
        }
    }


    public override void Open() {
        holder.SetActive(true);
        Time.timeScale = 0;
        isOpen = true;
    }
    public override void Close() {
        holder.SetActive(false);
        Time.timeScale = 1;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : Menu
{
    static PauseMenu singleton;
    void Awake() {
        if (singleton == null) {
            singleton = this;
            gameObject.SetActive(false);
        } else {
            Destroy(gameObject);
        }
    }
    public void Resume() {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public override void Open() {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void ReturnToMainMenu() {
        SceneManager.LoadScene("Title");

    }

    public static PauseMenu Instance() {
        return singleton;
    }
}

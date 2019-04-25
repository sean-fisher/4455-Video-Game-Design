using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pedestal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            LoadNextLevel();
        }
    }

    void LoadNextLevel() {
        LevelName[] levels = LevelManager.Instance().levels;
        string currLevelName = SceneManager.GetActiveScene().name;
        for (int i = 0; i < levels.Length; i++) {
            string levelName = levels[i].sceneName;
            if (levelName == currLevelName) {
                if (i == levels.Length - 1) {
                    // we've reached the last level!
                    Transitioner.Instance.LoadSceneWithFades("Title");
                } else {
                    string nextLevel = levels[i+1].sceneName;
                    Transitioner.Instance.LoadSceneWithFades(nextLevel);
                }
                this.enabled = false;
            }
        }
    }
}

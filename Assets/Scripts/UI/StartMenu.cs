using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : Menu
{

    // Start is called before the first frame update
    void Start()
    {
        AssignLevelButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject levelButtonTemplate;
    public Transform levelbuttonHolder;

    public void StartGame() {
        Transitioner.Instance.LoadSceneWithFades("PlaytestAlpha");
    }
    public void OpenCredits() {
        
    }
    public void OpenAbout() {
        
    }

    void AssignLevelButtons() {
        foreach (Transform child in levelbuttonHolder) {
            Destroy(child.gameObject);
        }
        string[] levels = LevelManager.Instance().levels;
        foreach (string name in levels) {
            GameObject newLevelButton = GameObject.Instantiate(levelButtonTemplate, levelbuttonHolder);
            newLevelButton.GetComponentInChildren<Text>().text = name;
            newLevelButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate {
                Transitioner.Instance.LoadSceneWithFades(name);
            });
        }
    }

    public void LoadLevel(int index) {
        string[] levels = LevelManager.Instance().levels;
        Transitioner.Instance.LoadSceneWithFades(levels[index]);
    }
}

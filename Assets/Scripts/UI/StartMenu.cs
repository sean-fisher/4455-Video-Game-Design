using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : Menu
{

    public UnityEngine.UI.Button backButton;

    // Start is called before the first frame update
    void Start()
    {
        AssignLevelButtons();
        EnterMenu();
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
        LevelName[] levels = LevelManager.Instance().levels;

        foreach (LevelName levelName in levels) {
            GameObject newLevelButton = GameObject.Instantiate(levelButtonTemplate, levelbuttonHolder);
            newLevelButton.GetComponentInChildren<Text>().text = levelName.visibleName;
            newLevelButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate {
                Transitioner.Instance.LoadSceneWithFades(levelName.sceneName);
            });
        }

        for (int i = 0; i < levels.Length; i++) {
            UnityEngine.UI.Button b = levelbuttonHolder.GetChild(i).GetComponent<UnityEngine.UI.Button>();
            Navigation nav = b.navigation;
            nav.mode = Navigation.Mode.Explicit;

            if (i == 0) {
                nav.selectOnUp = backButton;

                if (levels.Length == 1) {
                    nav.selectOnDown = backButton;

                } else {
                    nav.selectOnDown = levelbuttonHolder.GetChild(i+1).GetComponent<UnityEngine.UI.Button>();

                }
            } else if (i == levels.Length - 1) {
                nav.selectOnUp = levelbuttonHolder.GetChild(i-1).GetComponent<UnityEngine.UI.Button>();
                nav.selectOnDown = backButton;
            } else {
                nav.selectOnUp = levelbuttonHolder.GetChild(i-1).GetComponent<UnityEngine.UI.Button>();
                nav.selectOnDown = levelbuttonHolder.GetChild(i+1).GetComponent<UnityEngine.UI.Button>();
            }

            b.navigation = nav;
        }

        Navigation nav2 = backButton.navigation;
        nav2.mode = Navigation.Mode.Explicit;
        nav2.selectOnDown = levelbuttonHolder.GetChild(0).GetComponent<UnityEngine.UI.Button>();
        nav2.selectOnUp = levelbuttonHolder.GetChild(levels.Length - 1).GetComponent<UnityEngine.UI.Button>();
        backButton.navigation = nav2;
    }

    public void LoadLevel(int index) {
        string level = LevelManager.Instance().levels[index].sceneName;
        Transitioner.Instance.LoadSceneWithFades(level);
    }
}

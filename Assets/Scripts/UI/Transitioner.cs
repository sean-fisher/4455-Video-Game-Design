using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transitioner : MonoBehaviour
{
    public Image fadeImage;
    public float secondsToFade = 1;

    public static Transitioner Instance {
        get {
            if (instance == null) {
                Debug.LogError("Tried to acces Transitioner before it instantiated!");
            }
            return instance;
        }
    } 
    
    static Transitioner instance;

    public void FadeToBlack(float timeToFade = 1) {
        StartCoroutine(FadingToColor(timeToFade));
    }
    public void FadeToScene() {
        StartCoroutine(FadingToColor());
    }

    public void LoadSceneWithFades(string sceneName, float timeToFadeIn = 1, float timeToFadeOut = 1, Color? color = null) {
        StartCoroutine(LoadingSceneWithFades(sceneName, timeToFadeIn, timeToFadeOut, color ?? Color.black));
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) {
            DontDestroyOnLoad(this);
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadingToColor(float timeToFadeIn = 1, Color? color = null) {

        Color initialColor = fadeImage.color;
        float progress = 0;

        Color c = color ?? new Color(0, 0, 0, 0);
        
        fadeImage.color = new Color(c.r, c.g, c.b, 0);
        fadeImage.enabled = true;

        float r, g, b, a;

        while (progress < 1) {
            progress += Time.deltaTime / secondsToFade;
            float antiProgress = 1 - progress;

            r =    (c.r * progress + initialColor.r * antiProgress); 
            g =    (c.g * progress + initialColor.g * antiProgress);
            b =    (c.b * progress + initialColor.b * antiProgress);
            a =    (c.a * progress + initialColor.a * antiProgress);

            fadeImage.color = new Color(r,g,b,a);
            yield return null;
        }
        fadeImage.color = new Color(c.r, c.g, c.b, 1);
    }

    IEnumerator LoadingSceneWithFades(string sceneName, float timeToFadeOut, float timeToFadeIn, Color color) {
        fadeImage.enabled = true;

        Color transparentColor = new Color(color.r, color.g, color.b, 0);
        fadeImage.color = transparentColor;

        yield return FadingToColor(timeToFadeOut, color);
        SceneManager.LoadScene(sceneName);
        yield return FadingToColor(timeToFadeIn, transparentColor);
        fadeImage.enabled = false;
    }
}

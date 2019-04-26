using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    static Menu currentSelectedMenu;

    GameObject objectToSelectFirst;

    public void QuitToDesktop() {
        Application.Quit();
    }
    public void EnterMenu() {

        if (objectToSelectFirst == null) {
            objectToSelectFirst = transform.GetChild(0).gameObject;
        }
        currentSelectedMenu = this;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(objectToSelectFirst);
    }

    public static void SelectObjectInCurrentMenuIfPossible() {
        if (currentSelectedMenu) {
            currentSelectedMenu.EnterMenu();
        }
    }

    public virtual void Open(){}
    public virtual void Close(){}
}

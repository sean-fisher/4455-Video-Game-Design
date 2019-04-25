using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamepadKeyboardInputSwitcher : MonoBehaviour
{

    public GameObject objectToSelectUponSwitchToController;
    static bool gamePadIsControlling = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!gamePadIsControlling 
        && (InputManager.getMotionForward() != 0 || InputManager.getMotionForward() != 0 )) {
            gamePadIsControlling = true;
            //EventSystem.current.SetSelectedGameObject(this.gameObject);

            Menu.SelectObjectInCurrentMenuIfPossible();

            Debug.Log("Switch to controller");

        } else if (gamePadIsControlling && Input.GetAxis("MouseX") != 0) {
            gamePadIsControlling = false;
            EventSystem.current.SetSelectedGameObject(null);
            Debug.Log("Switch to keyboard/mouse");
        }
    }
}

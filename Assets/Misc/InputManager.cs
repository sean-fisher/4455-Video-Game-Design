using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {

    public static float getTotalMotionMag()
    {
        float x = getMotionHorizontal();
        float y = getMotionForward();

        return Mathf.Clamp01(Mathf.Sqrt(x * x + y * y));
    }
    
    public static float getMotionForward()
    {
        return Mathf.Clamp(Input.GetAxis("LeftStickY") + Input.GetAxisRaw("KeyboardY"), -1, 1);
    }
    
    public static float getMotionHorizontal()
    {
        return Mathf.Clamp(Input.GetAxis("LeftStickX") + Input.GetAxisRaw("KeyboardX"), -1, 1);
    }

    public static float getCameraX()
    {
        return Mathf.Clamp(Input.GetAxis("RightStickX") + Input.GetAxis("MouseX"), -1, 1);
    }

    public static float getCameraY()
    {
        return Mathf.Clamp(Input.GetAxis("RightStickY") + Input.GetAxis("MouseY"), -1, 1);
    }

    public static bool getJump()
    {
        return Input.GetButtonDown("Jump");
    }

    public static bool getRoll()
    {
        return Input.GetButtonDown("Roll");
    }
    public static bool getPause()
    {
        return Input.GetButtonDown("Pause");
    }

    public static bool getRightStickClick()
    {
        return Input.GetButtonDown("RightStickClick");
    }

    public static Vector3 calculateMove(float v, float h)
    {
        Vector3 ver = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized * v;
        Vector3 hor = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized * h;

        Vector3 move = hor + ver;
        move = Vector3.ProjectOnPlane(move, Vector3.up);
        move = Vector3.ClampMagnitude(move, 1);

        return move;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS
{
    public class CameraFreeStrategy : CameraStrategy
    {

        public override void ExecuteStrategyLateUpdate(PlayerCameraController camControl)
        {
            float dt = Time.deltaTime * 60f;

            // updates position.
             Follow(camControl);

            // Grabs Left Stick and Mouse Movement Inputs.
            float h = InputManager.getCameraX();
            float v = InputManager.getCameraY();

            // Adds Rotation scaled with rotXSpeed and rotYSpeed. rotXTar uses vertical input
            // as determines pitch, up and down, about the x-axis. The pitch is then clamped
            // between our min and max values. rotYTar uses horizontal input as it determines
            // yaw, side to side, about the y-axis.
            camControl.rotXTar += v * camControl.rotXSpeed * dt;
            camControl.rotYTar += h * camControl.rotYSpeed * dt;
            camControl.rotXTar = Mathf.Clamp(camControl.rotXTar, camControl.rotXmin, camControl.rotXmax);

            // uses Euler to set the transform rotation.
            camControl.camX.localRotation = Quaternion.Euler(0, camControl.rotYTar, 0);
            camControl.camY.localRotation = Quaternion.Euler(camControl.rotXTar, 0, 0);

            //AvoidWalls(camControl);
        }

        public void Follow(PlayerCameraController camControl)
        {
            float dt = Time.deltaTime * 60f;

            Vector3 currentPosition = camControl.transform.position;
            Vector3 targetPosition = camControl.followTarget.position;

            float newX = Mathf.Lerp(currentPosition.x, targetPosition.x, (1/ camControl.getXZDamp()) * dt);
            float newZ = Mathf.Lerp(currentPosition.z, targetPosition.z, (1 / camControl.getXZDamp()) * dt);
            float newY = Mathf.Lerp(currentPosition.y, targetPosition.y, (1 / camControl.getYDamp()) * dt);

            camControl.transform.position = new Vector3(newX, newY, newZ);
        }
    }
}
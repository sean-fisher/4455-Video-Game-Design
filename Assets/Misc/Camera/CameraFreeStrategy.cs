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

            // Grabs Left Stick and Mouse Movement Inputs.
            float h = InputManager.getCameraX();
            float v = InputManager.getCameraY();

            float rotX = v * camControl.rotXSpeed * dt;
            float rotY = h * camControl.rotYSpeed * dt;
            rotX = Mathf.Clamp(rotX, camControl.rotXMin, camControl.rotXMax);


            Transform camHolder = ((PlayerCameraController)camControl).camHolder;

            // add the rotation input to the camera's rotation
            // multiplication of Quaternions is like adding euler angles.
            Quaternion newRotation =
                camHolder.localRotation
                * Quaternion.Euler(v * camControl.rotXSpeed * dt, h * camControl.rotYSpeed * dt, 0);

            // I'm not 100% sure why the previous Quaternion.Euler(...) makes a Quaternion with nonzero euler z value
            // So this zeroes out the z rotation. 
            newRotation = Quaternion.Euler(newRotation.eulerAngles.x, newRotation.eulerAngles.y, 0);
            camHolder.localRotation = newRotation;

            // updates position.
            Follow(camControl);

            // Avoids walls
            PlaceCamera((PlayerCameraController)camControl);
        }

        private void Follow(PlayerCameraController camControl)
        {
            float dt = Time.deltaTime * 60f;

            Vector3 currentPosition = camControl.transform.position;
            Vector3 targetPosition = camControl.followTarget.position;

            float newX = Mathf.Lerp(currentPosition.x, targetPosition.x, (1 / camControl.xzDamp) * dt);
            float newZ = Mathf.Lerp(currentPosition.z, targetPosition.z, (1 / camControl.xzDamp) * dt);
            float newY = Mathf.Lerp(currentPosition.y, targetPosition.y, (1 / camControl.yDamp) * dt);

            camControl.transform.position = new Vector3(newX, newY, newZ);
        }

        private void PlaceCamera(PlayerCameraController camControl)
        {

            int mask = LayerMask.GetMask("Player");
            Vector3 pos = camControl.transform.position + new Vector3(0, camControl.yOffset, 0);
            Vector3 dir = camControl.camHolder.transform.forward * camControl.zOffset;
            dir = camControl.transform.TransformDirection(dir);
            RaycastHit rayHit;
            if (Physics.Raycast(pos, dir, out rayHit, -camControl.zOffset, mask))
            {
                Debug.DrawRay(pos, dir, Color.green);
                camControl.cam.position = pos + rayHit.distance * dir.normalized;
                return;
            }
            else
            {
                Debug.DrawRay(pos, dir, Color.red);
                camControl.cam.position = camControl.transform.position + new Vector3(0, camControl.yOffset, 0) + camControl.camHolder.transform.forward * camControl.zOffset;
                return;
            }
        }
    }
}
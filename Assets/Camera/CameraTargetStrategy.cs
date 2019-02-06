using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS
{
    public class CameraTargetStrategy : CameraStrategy
    {

        private Quaternion targetRotation;
        private float progress;
        public void SetTargetRotation(Quaternion rot, PlayerCameraController camControl) {
            targetRotation = Quaternion.RotateTowards(camControl.transform.rotation, rot, 1);
            camControl.rotXTar = targetRotation.eulerAngles.x;
            camControl.rotYTar = targetRotation.eulerAngles.y;
            
        }

        public CameraTargetStrategy(Quaternion targetRotation, PlayerCameraController camControl) {
            SetTargetRotation(targetRotation, camControl);
        }

        public override void ExecuteStrategyLateUpdate(PlayerCameraController camControl)
        {
            Follow(camControl);

            progress += Time.deltaTime * camControl.targetSpeed;
            if (progress > 1) progress = 1;

            
            float lerpedRotX = Mathf.Lerp(
                    camControl.camX.localRotation.y, 
                    camControl.rotYTar, 
                    progress);
            float lerpedRotY = Mathf.Lerp(
                    camControl.camY.localRotation.x, 
                    camControl.rotXTar, 
                    progress);


            // uses Euler to set the transform rotation.
            camControl.camX.localRotation = Quaternion.Euler(
                0, 
                lerpedRotX,
                0);
                
            camControl.camY.localRotation = Quaternion.Euler(
                lerpedRotY,
                0,
                0);


            if (progress >= 1) {
                
                // we've finished pointing the camera where we wanted to
                camControl.SetStrategy(new CameraFreeStrategy());
                Debug.Log("Free camera");
            }
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS
{
    public class CameraTargetStrategy : CameraStrategy
    {
        private Quaternion targetRotation;
        private Quaternion startRotation;
        private float progress;

        public void SetTargetRotation(Quaternion targetRot, PlayerCameraController camControl)
        {
            startRotation = camControl.camHolder.rotation;
            targetRotation = targetRot;
        }

        public CameraTargetStrategy(Quaternion targetRotation, PlayerCameraController camControl)
        {
            SetTargetRotation(targetRotation, camControl);
        }

        public override void ExecuteStrategyLateUpdate(PlayerCameraController camControl)
        {
            Follow(camControl);

            PlayerCameraController controller = ((PlayerCameraController)camControl);

            progress += Time.deltaTime * camControl.targetSpeed;
            float evaluation = controller.targetSwingCurve.Evaluate(progress);
            if (progress > 1) progress = 1;

            Transform camHolder = controller.camHolder;

            Quaternion newRotation = Quaternion.Slerp(camHolder.rotation, targetRotation, evaluation);
            camHolder.rotation = newRotation;


            if (progress >= 1)
            {

                // we've finished pointing the camera where we wanted to
                camControl.SetStrategy(new CameraFreeStrategy());
                progress = 0;
            }
        }

        public void Follow(PlayerCameraController camControl)
        {
            float dt = Time.deltaTime * 60f;

            Vector3 currentPosition = camControl.transform.position;
            Vector3 targetPosition = camControl.followTarget.position;

            float newX = Mathf.Lerp(currentPosition.x, targetPosition.x, (1 / camControl.xzDamp) * dt);
            float newZ = Mathf.Lerp(currentPosition.z, targetPosition.z, (1 / camControl.xzDamp) * dt);
            float newY = Mathf.Lerp(currentPosition.y, targetPosition.y, (1 / camControl.yDamp) * dt);

            camControl.transform.position = new Vector3(newX, newY, newZ);
        }
    }
}
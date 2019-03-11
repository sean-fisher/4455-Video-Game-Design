using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS
{
    public class PlayerCameraController : MonoBehaviour
    {

        #region variables

        private CameraStrategy camStrat;
        public Transform camHolder;
        public Transform cam;
        public AnimationCurve targetSwingCurve;

        public Transform followTarget;

        public float yOffset = 1.5f;
        public float zOffset = -4.5f;

        public float rotXSpeed = 5f;
        public float rotYSpeed = 5f;
        public float targetSpeed = .2f;
        public float xzDamp = 10f;
        public float yDamp = 10f;
        public float rotXMin = -20;
        public float rotXMax = 80;

        public float traumaLevel = 0;

        #endregion

        // Use this for initialization
        void Start()
        {
            // start with free cam
            camStrat = new CameraFreeStrategy();
            camHolder = transform.GetChild(0);
            cam = camHolder.transform.GetChild(0);

        }

        private void LateUpdate()
        {
            if (InputManager.getRightStickClick())
            {
                CenterCamera();
            }

            if (camStrat != null)
                camStrat.ExecuteStrategyLateUpdate(this);
        }

        public void CenterCamera()
        {
            SetStrategy(new CameraTargetStrategy(followTarget.GetChild(0).rotation, this));
        }

        public void SetStrategy(CameraStrategy cameraStrategy)
        {
            camStrat = cameraStrategy;
        }
    }

    public abstract class CameraStrategy
    {
        public abstract void ExecuteStrategyLateUpdate(PlayerCameraController camControl);
    }
}
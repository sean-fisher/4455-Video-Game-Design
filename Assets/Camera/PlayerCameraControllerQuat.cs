using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS
{
    public class PlayerCameraControllerQuat : PlayerCameraController
    {

        #region variables

        public Transform camHolder;
        public AnimationCurve targetSwingCurve;

        #endregion
        
        // Use this for initialization
        void Start()
        {
            // start with free cam
            camStrat = new CameraFreeStrategyQuat();

            camHolder = transform.GetChild(0);
            cam = camHolder.GetComponent<Camera>();

        }

        private void LateUpdate()
        {
            
            if (InputManager.getRightStickClick()) {
                SetStrategy(new CameraTargetStrategyQuat(followTarget.GetChild(0).rotation, this));
            }

            if (camStrat != null) camStrat.ExecuteStrategyLateUpdate(this);
        }

        public void CenterCamera() {
            SetStrategy(new CameraTargetStrategyQuat(followTarget.GetChild(0).rotation, this));
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS
{
    public class PlayerCameraController : MonoBehaviour
    {

        #region variables

        [SerializeField]
        private float xzDamping = 10;
        [SerializeField]
        private float yDamping = 10;

        public Transform followTarget;

        [HideInInspector]
        public Transform camX;
        [HideInInspector]
        public Transform camY;
        [HideInInspector]
        public Transform camZoom;

        [HideInInspector]
        public CameraStrategy camStrat;
        [HideInInspector]
        private Camera cam;

        [HideInInspector]
        public float rotX;
        [HideInInspector]
        public float rotY;
        [HideInInspector]
        public float rotXTar;
        [HideInInspector]
        public float rotYTar;
        
        public float rotXmax = 65f;
        public float rotXmin = -20;

        [HideInInspector]
        public float rotXSpeed = 2;
        [HideInInspector]
        public float rotYSpeed = 2;

        #endregion
        
        // Use this for initialization
        void Start()
        {
            // start with free cam
            camStrat = new CameraFreeStrategy();

            camX = transform.GetChild(0);
            camY = camX.GetChild(0);
            camZoom = camY.GetChild(0);
            cam = camZoom.GetChild(0).gameObject.GetComponent<Camera>();

            rotXTar = camX.rotation.eulerAngles.x;
            rotYTar = camY.rotation.eulerAngles.y;
            rotX = rotXTar;
            rotY = rotYTar;
        }

        private void LateUpdate()
        {
            camStrat.ExecuteStrategyLateUpdate(this);
        }

        public void SetStrategy(CameraStrategy cameraStrategy)
        {
            camStrat = cameraStrategy;
        }

        public Camera GetCam()
        {
            return cam;
        }

        public float getXZDamp() { return xzDamping; }

        public float getYDamp() { return yDamping; }
    }

    public abstract class CameraStrategy
    {
        public abstract void ExecuteStrategyLateUpdate(PlayerCameraController camControl);
    }
}
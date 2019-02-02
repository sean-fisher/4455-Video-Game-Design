using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class Protag : Character<ProtagState, ProtagInput>
    {
        #region variables

        [Header("Hitboxes")]
        public Collider[] hurtBoxes;
        public Collider[] hitBoxes;

        [Header("Movement Settings")]
        public float jumpStrength = 10;
        public float aerialMovementStrength;
        public float aerialDrag;
        public float climbSpeed;
        
        [SerializeField]
        private AnimationCurve compressionCurve;

        [HideInInspector]
        public Rigidbody rb;
        [HideInInspector]
        public CapsuleCollider col;
        [HideInInspector]
        public Animator anim;
    
        public Vector3 chestOffset;
        public Transform modelTransform;
        
        private int selfMask;
        private bool vuln;
        private bool aerial;

        private bool grounded;
        private Vector3 groundNormal;
        
        private bool climbableWallInFront;
        private Vector3 climbableWallNormal;
        private Vector3 wallAnchorPosition;

        #endregion

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<CapsuleCollider>();
            anim = GetComponentInChildren<Animator>();
            modelTransform = transform.GetChild(0);
            climbableWallNormal = Vector3.up;
            
            anim.applyRootMotion = true;
            vuln = true;
            grounded = true;
            aerial = false;

            selfMask = ~ LayerMask.GetMask("Player");

            newState<ProtagLocomotionState>();

            // enable hurtboxes
            foreach (Collider hb in hurtBoxes)
                hb.enabled = true;

            // disable hitboxes
            foreach (Collider hb in hitBoxes)
                hb.enabled = false;

            chestOffset = new Vector3(0, col.height / 2, 0);
        }

        public override void readInput ()
        {
            input.v = InputManager.getMotionForward();
            input.h = InputManager.getMotionHorizontal();
            input.totalMotionMag = InputManager.getTotalMotionMag();
            input.jump = InputManager.getJump();
        }

        public void checkGround()
        {
            Vector3 pos = transform.position + (Vector3.up * 0.5f);
            Vector3 dir = (Vector3.down * 0.8f);
            RaycastHit groundCheck;
            if (Physics.Raycast(pos, dir, out groundCheck, 0.8f, selfMask))
            {
                Debug.DrawRay(pos, dir, Color.green);
                grounded = true;
                groundNormal = groundCheck.normal;
                return;
            }
            else
            {
                Debug.DrawRay(pos, dir, Color.red);
                grounded = false;
                groundNormal = Vector3.up;
                return;
            }
        }

        public void checkClimableWallInFront() {

            float rayLength = col.radius * 2;
            Vector3 start = chestOffset + transform.localPosition;
            Vector3 dir = modelTransform.forward;

            RaycastHit hit;
            Debug.DrawRay(start, dir * rayLength, Color.red);
            // Is there something in front?
            if (Physics.Raycast(start, dir, out hit, rayLength, selfMask)) {

                // is it a climbable wall?
                if (hit.transform.gameObject.CompareTag("Climbable"))
                    climbableWallInFront = true;
                else
                    climbableWallInFront = false;

            }
            else
            {
                climbableWallInFront = false;
            }
        }
        
        public Vector3 checkClimbingWall() {

            Vector3 wallNormal = -modelTransform.forward;

            climbableWallInFront = false;

            Vector3 start = transform.localPosition;
            Vector3 dir = modelTransform.forward;
            
            List<Vector3> hitPoints = new List<Vector3>();
            List<Vector3> hitNormals = new List<Vector3>();

            // check at the head of the player
            start = transform.localPosition + chestOffset.magnitude * modelTransform.up;
            RaycastHit hit;
            if (Utility.RayCastInArc(out hit, start, modelTransform.up, modelTransform.right, col.height / 2, 90, Color.green, selfMask, 4)) {
                // there is a climbable wall above
                hitPoints.Add(hit.point);
                hitNormals.Add(hit.normal);
            }

            // check at the feet of the player
            start = transform.localPosition  + chestOffset.magnitude * modelTransform.up;
            if (Utility.RayCastInArc(out hit, start, -modelTransform.up, -modelTransform.right, col.height / 2, 90, Color.red, selfMask, 4)) {
                // there is a climbable wall below
                hitPoints.Add(hit.point);
                hitNormals.Add(hit.normal);
            }

            start = transform.position;
            RaycastHit wallAnchorCheck;
            if (Physics.Raycast(start, modelTransform.forward, out wallAnchorCheck, 0.5f, selfMask))
            {
                Debug.DrawRay(start, dir, Color.blue);
                wallAnchorPosition = wallAnchorCheck.point;
            }

            // find the average wall normal of the points we can reach
            Vector3 vecSum = Vector3.zero;
            foreach (Vector3 norm in hitNormals) {
                vecSum += norm;
            }
            wallNormal = vecSum / hitNormals.Count;


            // find the center of the points we can reach
            vecSum = Vector3.zero;
            foreach (Vector3 point in hitPoints) {
                vecSum += point;
            }

            return wallNormal;
        }

        public bool isMovingForward() {
            Vector3 move = InputManager.calculateMove(input.v, input.h);
            return Mathf.Abs((move.normalized - modelTransform.forward).magnitude) < 1;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (aerial)
            {
                Vector3 pos = transform.position + (Vector3.up * 0.5f);
                Vector3 dir = (Vector3.down * 0.8f);
                RaycastHit groundCheck;
                if (Physics.Raycast(pos, dir, out groundCheck, 0.8f, selfMask))
                {
                    Debug.DrawRay(pos, dir, Color.green);
                    grounded = true;
                    groundNormal = groundCheck.normal;
                }
                else
                {
                    Debug.DrawRay(pos, dir, Color.red);
                    grounded = false;
                    groundNormal = Vector3.up;
                }
            }
        }

        public float sampleCompressionCurve(float x) { return compressionCurve.Evaluate(x); }

        public bool getGrounded() { return grounded; }

        public bool getVulnerable() { return vuln; }
        
        public bool getIsClimbableWallInFront() { return climbableWallInFront; }

        public Vector3 getGroundNormal() { return groundNormal; }

        public Vector3 getClimableWallNormal() { return climbableWallNormal; }

        public Vector3 getWallAnchorPosition() { return wallAnchorPosition; }

        public void setGrounded(bool value) { grounded = value; }

        public void setVulnerable(bool value) { vuln = value; }

        public void setRootMotion(bool value) { anim.applyRootMotion = value; }

        public void setAerial(bool value) { aerial = value; }
    }

    public class ProtagInput : CharacterInput
    {
        public float v;
        public float h;
        public float totalMotionMag;

        public bool jump;
    }
}

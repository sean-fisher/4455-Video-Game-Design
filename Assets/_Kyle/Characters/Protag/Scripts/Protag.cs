using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public class Protag : Character<ProtagState, ProtagInput>, Damageable
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
        public float rotationOrientSpeed = 10;
        [HideInInspector]
        public bool isVulnerable;
        
        [SerializeField]
        private AnimationCurve compressionCurve;
        [SerializeField]
        private AnimationCurve climpUpCurve;

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
        public bool climbing;
        private Vector3 groundNormal;
        
        private bool climbableWallInFront;
        private Vector3 climbableWallNormal;
        private Vector3 wallAnchorPosition;
        private ClimbingContextualActionType nextClimbingAction;
        public float yVecSpeed;

        #endregion

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            //col = GetComponent<CapsuleCollider>();
            col = transform.GetChild(0).GetComponent<CapsuleCollider>();
            anim = GetComponentInChildren<Animator>();
            modelTransform = transform.GetChild(0);
            climbableWallNormal = Vector3.up;
            isVulnerable = true;
            
            anim.applyRootMotion = true;
            vuln = true;
            grounded = true;
            aerial = false;

            selfMask = ~ LayerMask.GetMask("Player");

            newState<ProtagLocomotionState>();
            //newState<ProtagHoverBikeState>();

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
            input.roll = InputManager.getRoll();
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

        public bool checkLedgeAbove() {
            // if we are at the base of a small cliff (or climbing at the top of a large cliff),
            // we raycast down from ahead and above us to check the ground to climb onto.

            float rayLength = 1;
            Vector3 start = chestOffset * 2 + transform.localPosition;
            Vector3 dir = Vector3.down;

            RaycastHit hit;
            Debug.DrawRay(start, dir * rayLength, Color.red);

            // Is there a cliff above and in front?
            if (Physics.Raycast(start, dir, out hit, rayLength, selfMask)) {

                // is it a very flat surface, as opposed to a gradual slope?
                if (Mathf.Abs(Vector3.Angle(hit.normal, Vector3.up)) < 10) {

                    // are we oriented vertically enough that the climbing up ledge animation would be appropriate?
                    if (Mathf.Abs(Vector3.Angle(modelTransform.forward, transform.forward)) < 15) {
                        return true;
                    }
                }
            }
            return false;
        }

        public void lerpRotationToUpwards(){

            // Orient the character so he is standing up

            // lerping to base orientation can be ugly if we don't choose the right thing to lerp to.
            // for example, if we're at -350 degrees rotation and want to get to 0, we should lerp to -360, not 0
            float xSign = modelTransform.rotation.eulerAngles.x < 0 ? -1 : 1;
            float rotationXTarget = (modelTransform.rotation.eulerAngles.x > 180 ? 360 : 0) * xSign;
            float zSign = modelTransform.rotation.eulerAngles.z < 0 ? -1 : 1;
            float rotationZTarget = (modelTransform.rotation.eulerAngles.z > 180 ? 360 : 0) * zSign;

            modelTransform.rotation = Quaternion.Euler(
                Mathf.Lerp(modelTransform.rotation.eulerAngles.x, rotationXTarget, Time.deltaTime * rotationOrientSpeed), 
                modelTransform.rotation.eulerAngles.y,
                Mathf.Lerp(modelTransform.rotation.eulerAngles.z, rotationZTarget, Time.deltaTime * rotationOrientSpeed));
        }
        
        public void checkClimbingWall() {

            climbableWallNormal = -modelTransform.forward;

            climbableWallInFront = false;

            Vector3 start = transform.localPosition;
            Vector3 dir = modelTransform.forward;
            
            List<Vector3> hitPoints = new List<Vector3>();
            List<Vector3> hitNormals = new List<Vector3>();

            // check at the head of the player
            start = transform.localPosition + chestOffset.magnitude * modelTransform.up / 4;
            RaycastHit hit;
            if (Utility.RayCastInArc(out hit, start, modelTransform.up, modelTransform.right, col.height / 2, 90, Color.green, selfMask, 4)) {
                // there is a climbable wall above
                
                // Is there a cliff above and in front?

                // is it a very flat surface, as opposed to a gradual slope?

                if (Mathf.Abs(Vector3.Angle(hit.normal, Vector3.up)) < 10) {
                    Vector3 movementDir = Vector3.ProjectOnPlane(modelTransform.forward, groundNormal);
                    // are we oriented vertically enough that the climbing up ledge animation would be appropriate?
                    if (Mathf.Abs(Vector3.Angle(modelTransform.forward, movementDir)) < 15) {
                        // we can climb up
                        wallAnchorPosition = hit.point;
                        climbableWallNormal = Vector3.zero;
                        nextClimbingAction = ClimbingContextualActionType.CLIMBUP;
                        return;
                    }
                } else {
                    hitPoints.Add(hit.point);
                    hitNormals.Add(hit.normal);

                }
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
            climbableWallNormal = hitNormals.Count > 0 ? vecSum / hitNormals.Count : Vector3.zero;


            // find the center of the points we can reach
            vecSum = Vector3.zero;
            foreach (Vector3 point in hitPoints) {
                vecSum += point;
            }
            wallAnchorPosition = hitPoints.Count > 0 ? vecSum / hitPoints.Count : Vector3.zero;

            nextClimbingAction = ClimbingContextualActionType.CLIMBING;
        }

        public bool isMovingForward() {
            Vector3 move = InputManager.calculateMove(input.v, input.h);
            return Mathf.Abs((move.normalized - modelTransform.forward).magnitude) < 1;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (aerial)
            {
                checkGround();
            }
        }

        public float sampleCompressionCurve(float x) { return compressionCurve.Evaluate(x); }
        public float sampleClimbUpCurve(float x) { return climpUpCurve.Evaluate(x); }

        public bool getGrounded() { return grounded; }
        public bool getClimbing() { return climbing; }

        public bool getVulnerable() { return vuln; }

        public int GetSelfMask() { return selfMask; }
        
        public bool getIsClimbableWallInFront() { return climbableWallInFront; }

        public Vector3 getGroundNormal() { return groundNormal; }

        public Vector3 getClimableWallNormal() { return climbableWallNormal; }

        public Vector3 getWallAnchorPosition() { return wallAnchorPosition; }

        public void setGrounded(bool value) { grounded = value; }

        public void setVulnerable(bool value) { vuln = value; }

        public void setRootMotion(bool value) { anim.applyRootMotion = value; }

        public void setAerial(bool value) { aerial = value; }
        public void setClimbableWallNormal(Vector3 wallNormal) {climbableWallNormal = wallNormal;}
        public ClimbingContextualActionType GetNextActionType() {return nextClimbingAction;}

        public void TakeDamage(Damage damage)
        {
            if (damage.type != DamageType.Protag && vuln)
            {
                input.dmg = damage;
            } 
        }

        public void CleanDamage()
        {
            input.dmg = null;
        }

        public bool IsVulnerable()
        {
            return isVulnerable;
        }
    }

    public class ProtagInput : CharacterInput
    {
        public Damage dmg;
        public float v;
        public float h;
        public float totalMotionMag;

        public bool jump;
        public bool roll;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS
{
    public static class Utility
    {
        public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
        {
            Vector3 perp = Vector3.Cross(fwd, targetDir);
            float dir = Vector3.Dot(perp, up);

            if (dir > 0.0f)
            {
                return 1.0f;
            }
            else if (dir < 0.0f)
            {
                return -1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

        public static bool RayCastInArc(
            out RaycastHit hit,
            Vector3 center,
            Vector3 radiusDir,
            Vector3 axis,
            float radius,
            float angleSpanDegrees,
            Color color,
            int selfMask,
            int numRays = 4)
        {

            float rayLength = 2 * 3.1415f * radius * (angleSpanDegrees / 360 / numRays);
            float angleStep = angleSpanDegrees / (float)numRays;
            Vector3 lastDir = Vector3.zero;
            Vector3 start = center + radiusDir * radius;

            // cast rays in an arc starting from center offset by radius until we hit something or span angleSpanDegrees
            for (int i = 0; i < numRays; i++)
            {

                // start the next ray from where the other ended
                start += lastDir * rayLength;
                Vector3 dirToCast = Vector3.Cross(axis, radiusDir);
                dirToCast = Quaternion.AngleAxis(angleStep * i, axis) * dirToCast;

                Debug.DrawRay(start, dirToCast * rayLength, color);

                if (Physics.Raycast(start, dirToCast, out hit, rayLength, selfMask) && hit.collider.gameObject.CompareTag("Climbable"))
                {
                    return true;
                }

                lastDir = dirToCast;
            }

            hit = new RaycastHit();
            return false;
        }
    }

    public class PointNormalActionTypeTuple {
        public Vector3 point;
        public Vector3 normal;
        public ClimbingContextualActionType actionType;

        public PointNormalActionTypeTuple(Vector3 p, Vector3 n, ClimbingContextualActionType type) {
            point = p;
            normal  = n;
            actionType = type;
        }
    }

    public enum ClimbingContextualActionType {
        CLIMBING,
        CLIMBUP,
        CLIMBDOWN
    }

}

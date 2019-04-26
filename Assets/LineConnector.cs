using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class LineConnector : MonoBehaviour
{
    public Transform object1;
    public Transform object2;
    public float subdivisionSize = 1;

    Vector3 lastPos;
    Vector3 targetlastPos;

    Transform lineHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if (UNITY_EDITOR)
        if (object1 != null && object2 != null) {
            if ((object1.position != lastPos) || (object2.position != targetlastPos)) {
                CreateLine();
                lastPos = object1.position;
                targetlastPos = object2.position;
            }
        }
#endif
    }
    Vector3[] points;

    public void CreateLine() {

        if (lineHolder == null) {
            //lineHolder = GameObject.Instantiate(new GameObject()).transform;
        }

        Vector3 endOnSamePlaneAsStart = new Vector3(object2.position.x, Mathf.Max(object1.position.y, object2.position.y), object2.position.z);

        float dist = (endOnSamePlaneAsStart - object1.position).magnitude;
        int numSubdivisions = (int) (dist / subdivisionSize);

        Vector3 start = object1.position;
        Vector3 end =   object2.position;

        points = new Vector3[numSubdivisions + 1];

        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();
        List<int> tris = new List<int>();
        Vector3[] allNormals = new Vector3[numSubdivisions + 1];
        allNormals[0] = Vector3.up;
        List<Vector3> importantNormals = new List<Vector3>();

        points[0] = start;

        for (int i = 0; i < numSubdivisions; i++) {
            Vector3 cylinderStart = start + (endOnSamePlaneAsStart-start).normalized * (i * subdivisionSize) + Vector3.up;
            Vector3 cylinderEnd   = start + (endOnSamePlaneAsStart-start).normalized * ((i + 1) * subdivisionSize) + Vector3.up;

            int layerMask = ~LayerMask.GetMask("Interactable");
            RaycastHit hit;
            if (Physics.Raycast(cylinderEnd, Vector3.down, out hit, 20, layerMask, QueryTriggerInteraction.Ignore)) {
                cylinderEnd = hit.point;
                allNormals[i+1] = hit.normal;
            }

            points[i+1] = cylinderEnd;
            //points.Add(cylinderEnd);
        }
        
        for (int i = 0; i < points.Length - 1; i++) {
            int aIndex = i;
            Vector3 a = points[aIndex];
            Vector3 b = points[i+1];
            Vector3 lastPoint = b;

            while ((Mathf.Abs(a.y- b.y) < .2f) && i < points.Length - 1) {
                lastPoint = b;
                b = points[i+1];

                if ((Mathf.Abs(a.y- b.y) > .2f)) {
                    i--;
                    break;
                }
                i++;
            }
            if (i == points.Length - 1 || i == points.Length) {
                i = points.Length - 1;
            }

            Vector3 normal;
            if (i == 0) {
                normal = allNormals[0];
            } else if (i < points.Length - 1) {
                Vector3 lastToThis = points[i] - points[i-1];
                Vector3 nextToThis = points[i] - points[i+1];
                normal = (lastToThis.normalized + nextToThis.normalized).normalized;

                // add an offset to avoid z-fighting

                if (Vector3.Dot(Vector3.down, normal) > 0) {
                    // the point is in a concave area
                    normal *= -1;
                } else {
                    // convex
                }
            } else {
                normal = allNormals[points.Length - 1];
            }

            Vector3 normalOffset = normal * .05f;

            Vector3 horizOffset = Vector3.Cross(endOnSamePlaneAsStart -lastPoint, Vector3.up).normalized * .1f;
            verts.Add(lastPoint + horizOffset + normalOffset);
            verts.Add(lastPoint - horizOffset + normalOffset);
            importantNormals.Add(normal);
        }
        
        float xStep = 1f / verts.Count;
        for (int i = 0; i < verts.Count; i++) {
            uv.Add(new Vector2(xStep * i, i%2 == 0 ? 0 : 1));
        }
            
        var mf = GetComponent<MeshFilter>();
        var mesh = new Mesh();
        mf.mesh = mesh;

        int numVerts = verts.Count;//verts.Count;
        
        var vertices = new Vector3[numVerts];
        var uv2 = new Vector2[numVerts];
        var tri = new int[(numVerts-2) * 3];// / 2 * 3];
        var normals = new Vector3[numVerts];

        
            
        vertices[0] = verts[0] - object1.position;// new Vector3(0, 0, 0);
        vertices[1] = verts[1] - object1.position;
        uv2[0] = new Vector2(0, 1);
        uv2[1] = new Vector2(1, 1);
        normals[0] = Vector3.up;
        normals[1] = Vector3.up;

        int iters = 0;
        for (int i = 2; i < numVerts; i+=2) {
            if (i == 0) {
            vertices[i+0] = verts[i+0] - object1.position;// new Vector3(0, 0, 0);
            vertices[i+1] = verts[i+1] - object1.position;
            }
            vertices[iters*2+2] = verts[iters*2+2] - object1.position;
            vertices[iters*2+3] = verts[iters*2+3] - object1.position;
            

            tri[iters*6+0] = iters*2;
            tri[iters*6+1] = iters*2+2;
            tri[iters*6+2] = iters*2+1;
            
            tri[iters*6+3] = iters*2+2;
            tri[iters*6+4] = iters*2+3;
            tri[iters*6+5] = iters*2+1;
            
            
            //normals[i+1] = -Vector3.forward;
            //normals[i+2] = -Vector3.forward;
            normals[iters*2+2] = -Vector3.forward;//importantNormals[iters*1+1];
            normals[iters*2+3] = -Vector3.forward;//importantNormals[iters*1+1];
            //normals[iters*2+2] = importantNormals[iters*1+1];
            //normals[iters*2+3] = importantNormals[iters*1+1];
            
            uv2[iters*2+2] = new Vector2(0, 1);
            uv2[iters*2+3] = new Vector2(1, 1);

            iters++;
        }
            
        
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = tri;
        
        mesh.uv = uv2;
    
    }

    void OnDrawGizmos() {

        //points = new List<Vector3>();
        //points.Add(object1.position);
        //points.Add(target.position);
        if (lineHolder) {

            if (points != null) {
            Gizmos.color = Color.red;
            for (int i = 0; i < points.Length - 1; i++) {
                Vector3 a = points[i];
                Vector3 b = points[i+1];
                Vector3 lastPoint = b;

                while (a.y == b.y && i < points.Length - 1) {
                    lastPoint = b;
                    b = points[i+1];

                    if (a.y != b.y) {
                        i--;
                        break;
                    }
                    i++;
                }
                Gizmos.DrawLine(a, lastPoint);
            }
            }
        }
    }
}

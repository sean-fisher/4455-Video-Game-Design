using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordRobot : MonoBehaviour
{
    Animator anim;
    public Transform target;
    public Collider[] hitBoxes;
    private NavMeshAgent navMeshAgent;
    private bool hasAttacked = false;

    public float rangeOfAttention = 5.0f;

    public EnemyState state;
    public Transform[] waypoints;
    public int currWaypoint = -1;

    // Start is called before the first frame update
    void Start()
    {
        state = EnemyState.Patrol;
        setNextWaypoint();
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        foreach (Collider hb in hitBoxes)
        {
            hb.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.Patrol) {
			if (navMeshAgent.path.status == NavMeshPathStatus.PathPartial) {
				state = EnemyState.Wait;
			}
            anim.SetBool("running", false);
            navMeshAgent.speed = 1.9f;
            float yDis = Mathf.Abs(target.transform.position.y - transform.position.y);
            if (yDis < 2.0f && Vector3.Distance(transform.position, target.position) < rangeOfAttention)
            {
                NavMeshPath path = new NavMeshPath();
                navMeshAgent.CalculatePath(target.position, path);
                if (path.status != NavMeshPathStatus.PathPartial)
                {
                    state = EnemyState.InterceptTarget;
                    navMeshAgent.SetDestination(target.transform.position);
                }
            }
            else if (navMeshAgent.remainingDistance < .5 && !navMeshAgent.pathPending)
            {
                setNextWaypoint();
            }
        }
        else if (state == EnemyState.InterceptTarget) {
            float xzDis = (Vector3.ProjectOnPlane(target.transform.position, Vector3.up) - Vector3.ProjectOnPlane(transform.position, Vector3.up)).magnitude;
            if (xzDis > rangeOfAttention || navMeshAgent.path.status == NavMeshPathStatus.PathPartial)
            {
                state = EnemyState.Patrol;
                setNextWaypoint();
            }
            transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(target.transform.position - transform.position, Vector3.up), Vector3.up);
            float yDis = Mathf.Abs(target.transform.position.y - transform.position.y);
            if (xzDis <= 1.5f && yDis <= .5f)
            {
                anim.SetTrigger("attack");
                hasAttacked = true;
            }
            else if (yDis > .5f)
            {
                anim.SetBool("running", false);
            }
            else
            {
                anim.SetBool("running", true);
                navMeshAgent.speed = 3f;
            }
            if (hasAttacked && !anim.GetBool("attack"))
            {
                navMeshAgent.SetDestination(target.transform.position);
            }
        }
		else if (state == EnemyState.Wait) {
			anim.SetBool("running", false);
			anim.SetBool ("wait", true);
			navMeshAgent.SetDestination (transform.position);
			NavMeshPath path = new NavMeshPath();
			navMeshAgent.CalculatePath(waypoints[currWaypoint].transform.position, path);
			if (path.status != NavMeshPathStatus.PathPartial)
			{
				state = EnemyState.Patrol;
				anim.SetBool ("wait", false);
				navMeshAgent.SetDestination(waypoints[currWaypoint].transform.position);
			}
		}
    }

    public void openHitboxes()
    {
        foreach (Collider hb in hitBoxes)
        {
            hb.enabled = true;
        }
    }

    public void closeHitboxes()
    {
        foreach (Collider hb in hitBoxes)
        {
            hb.enabled = false;
        }
    }

    private void setNextWaypoint()
    {
        try
        {
            currWaypoint = (currWaypoint + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[currWaypoint].transform.position);
        }
        catch
        {
            Debug.Log("Next Waypoint cannot be set due to array indexing issue or array is of length 0 ");
        }
    }
}



public enum EnemyState
{
    Patrol,
    InterceptTarget,
    Wait
};


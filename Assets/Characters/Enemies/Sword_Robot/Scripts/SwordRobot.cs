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

	public float walkSpeed = 1.9f;
	public float runSpeed = 4.5f;

    public float rangeOfAttention = 5.0f;

    public EnemyState state;
    public Transform[] waypoints;
    public int currWaypoint = -1;

	public AudioSource walkingSound;
	public AudioSource runningSound;
	public AudioSource targetIdentified;

	private float wasSpeed = 0.0f;

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

		runningSound.Stop ();
		walkingSound.Stop (); 
		targetIdentified.Stop ();
    }

    // Update is called once per frame
    void Update()
    {
		//Audio Handling
		if (wasSpeed != 0 && (navMeshAgent.speed == 0.0f || state == EnemyState.Wait) ) {
			Debug.Log ("no audio");
			wasSpeed = 0.0f;
			runningSound.Stop ();
			walkingSound.Stop (); 
		} else if (wasSpeed != walkSpeed && navMeshAgent.speed == walkSpeed && state != EnemyState.Wait) {
			Debug.Log ("walking audio " + wasSpeed + ": " + navMeshAgent.speed);
			wasSpeed = walkSpeed;
			runningSound.Stop ();
			walkingSound.loop = true;
			walkingSound.Play ();

		} else if (wasSpeed != runSpeed && navMeshAgent.speed == runSpeed) {
			Debug.Log ("running audio");
			wasSpeed = runSpeed;
			walkingSound.Stop ();
			runningSound.loop = true;
			runningSound.Play ();
		}


		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
			Debug.Log("attacking");
			navMeshAgent.SetDestination (transform.position);
			hasAttacked = false;
		}
        else if (state == EnemyState.Patrol) {
			if (navMeshAgent.path.status == NavMeshPathStatus.PathPartial) {
				state = EnemyState.Wait;
			}
            anim.SetBool("running", false);
			navMeshAgent.speed = walkSpeed;
            float yDis = Mathf.Abs(target.transform.position.y - transform.position.y);
            if (yDis < 2.0f && Vector3.Distance(transform.position, target.position) < rangeOfAttention)
            {
                NavMeshPath path = new NavMeshPath();
                navMeshAgent.CalculatePath(target.position, path);
                if (path.status != NavMeshPathStatus.PathPartial)
                {
					targetIdentified.Play ();
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
            if (!hasAttacked && xzDis <= 1.5f && yDis <= .5f)
            {
                SoundManager.Instance.PlayAnySFX("Goblin");
				navMeshAgent.SetDestination (transform.position);
                anim.SetTrigger("attack");
				hasAttacked = true;
            }
            else if (yDis > .5f)
            {
				navMeshAgent.speed = walkSpeed;
                anim.SetBool("running", false);
				navMeshAgent.SetDestination(target.transform.position);
            }
            else
            {
                
                anim.SetBool("running", true);
                navMeshAgent.speed = runSpeed;
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


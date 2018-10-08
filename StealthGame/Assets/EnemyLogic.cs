using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour {

    public EnemyStates state = EnemyStates.idle;

    Transform navTarget = null;

    NavMeshAgent agent;

    public Transform[] sight;

    public Animator anim;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
        switch(state)
        {
            case EnemyStates.idle:
                UpdateIdle();
                break;
            case EnemyStates.investigate:
                UpdateInvestigate();
                break;
            case EnemyStates.patrol:
                UpdatePatrol();
                break;
            case EnemyStates.pursue:
                UpdatePursue();
                break;
            case EnemyStates.suspicious:
                UpdateSuspicious();
                break;
        }

        
	}

    void UpdateIdle()
    {
        anim.SetBool("idle", true);
        anim.SetBool("running", false);

        if (PlayerInSight())
        {
            state = EnemyStates.pursue;
            navTarget = GameObject.Find("Player").transform;
        }
    }

    void UpdateInvestigate()
    {

    }

    void UpdatePatrol()
    {

    }

    void UpdatePursue()
    {
        anim.SetBool("idle", false);
        anim.SetBool("running", true);

        agent.SetDestination(navTarget.position);
    }

    void UpdateSuspicious()
    {

    }

    bool PlayerInSight()
    {
        /*bool inSight = false;

        Vector3 vOffset = new Vector3(0, 0.5f, 0) ;

        Vector3 rayStart = transform.position + vOffset;

        Ray forward = new Ray(rayStart, transform.forward);
        Ray forwardLeft = new Ray(rayStart, transform.forward + -transform.right);
        Ray forwardRight = new Ray(rayStart, transform.forward + transform.right);


        RaycastHit hit;
        float distance = 5.0f;
        if(Physics.Raycast(forward, out hit, distance))
        {
            inSight = true;
        }
        else if (Physics.Raycast(forwardLeft, out hit, distance))
        {
            inSight = true;
        }
        else if (Physics.Raycast(forwardRight, out hit, distance))
        {
            inSight = true;
        }

        Debug.Log(inSight);
        return inSight;*/
        Debug.Log("test");
        foreach(Transform t in sight)
        {
            Debug.DrawLine(t.position, t.position + t.forward * 10, Color.red);
            RaycastHit hit;
            if(Physics.Raycast(t.position, t.forward, out hit, 10))
            {
                if (hit.transform.tag == "Player")
                {
                    Debug.Log("hit");
                    return true;
                }
            }
        }

        return false;
    }

    public enum EnemyStates
    {
        idle, // just standing
        suspicious, // stand in place, but look in direction of disturbance
        investigate, // go to point of disturbance
        pursue, // follow player
        patrol // move along path
    }
}

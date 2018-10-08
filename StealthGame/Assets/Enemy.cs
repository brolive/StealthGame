using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Transform player;

    public float alertDistance = 4.0f;

    bool alert = false;

    public GameObject displayAlert;

    public EnemyStates state = EnemyStates.idle;

    public float sightDistance = 5.0f;

    public float followSpeed = 3.0f;

    Vector3 moveDirection;

	// Use this for initialization
	void Start () {

        moveDirection.y = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		
        switch(state)
        {
            case EnemyStates.idle:
                UpdateIdle();
                break;
            case EnemyStates.patrol:
                UpdatePatrol();
                break;
            case EnemyStates.alert:
                UpdateAlert();
                break;
            case EnemyStates.follow:
                UpdateFollow();
                break;
        }

        transform.Translate(moveDirection * Time.deltaTime);

	}

    // just stand
    public void UpdateIdle()
    {
        if(InLineOfSight())
        {
            state = EnemyStates.follow;
            return;
        }
    }

    // walk around
    public void UpdatePatrol()
    {
       
    }

    // heard something or saw somethin briefly
    public void UpdateAlert()
    {

    }

    // hunt down the player
    public void UpdateFollow()
    {
        Vector3 newMove = (player.position - transform.position) * followSpeed;
        newMove.y = moveDirection.y;
        moveDirection = newMove;
    }

    bool InLineOfSight()
    {
        Vector3 eyeL = transform.position + Vector3.up * 1.75f + Vector3.left * .2f;
        Vector3 eyeR = transform.position + Vector3.up * 1.75f + Vector3.right * .2f;

        RaycastHit hit;

        bool hitL = false;
        bool hitR = false;

        Debug.DrawLine(eyeL, eyeL + -transform.forward * sightDistance);

        if (Physics.Raycast(eyeL, -transform.forward, out hit, sightDistance))
        {
            Debug.Log("left");
            hitL = true;
        }

        if (Physics.Raycast(eyeR, -transform.forward, out hit, sightDistance))
        {
            Debug.Log("right");
            hitR = true;
        }

        //Debug.Log(hitL);
        //Debug.Log(hitR);
        return (hitL || hitR);
    }

    public enum EnemyStates
    {
        idle,
        patrol,
        alert,
        follow
    }
}

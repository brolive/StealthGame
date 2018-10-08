using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public float distanceBack = 0.0f;
    public float distanceUp = 0.0f;
    public float distanceSide = 0.0f;
    public float camSpeed = 1.0f;

    public Transform player;

    public CameraStates state;

    public float rotSpeed = 1.0f;

	// Use this for initialization
	void Start () {
        state = CameraStates.follow;
	}
	
	// Update is called once per frame
	void Update () {

        /*Vector3 offset = new Vector3(distanceSide, distanceUp, distanceBack);

        transform.position = Vector3.Lerp(transform.position, 
                                          player.position + offset, 
                                          camSpeed * Time.deltaTime);*/

        /*Vector3 offset = player.position +
                             (player.forward * distanceBack) +
                             (player.up * distanceUp);
                             //(player.right * distanceSide);

        transform.position = Vector3.Lerp(transform.position, offset, camSpeed * Time.deltaTime);
        transform.LookAt(GameObject.Find("Player").transform);*/

        //transform.position = Vector3.Lerp(transform.position, player.position + player.forward * distanceBack, .1f * Time.deltaTime);
        //transform.LookAt(GameObject.Find("Player").transform);

        

        switch(state)
        {
            case CameraStates.follow:
                UpdateFollow();
                break;
            case CameraStates.orbit:
                UpdateOrbit();
                break;
        }
    }

    public void UpdateFollow()
    {
        Vector3 lookDir = player.position - transform.position;
        // kill y
        lookDir.y = 0;
        // normalize to give a valid direction
        lookDir.Normalize();


        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        
        transform.position = player.position +
                             lookDir * distanceBack +
                             Vector3.up * distanceUp;/* + 
                             Vector3.right * distanceSide;*/

        transform.LookAt(player);

        transform.RotateAround(player.position, Vector3.up, h * rotSpeed * Time.deltaTime);
        //transform.RotateAround(player.position, Vector3.right, v * rotSpeed * Time.deltaTime);
        distanceUp += v * Time.deltaTime * (rotSpeed * 0.05f);
    }

    public void UpdateOrbit()
    {
        /*float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        if(h == 0)
        {
            transform.parent = null;
            state = CameraStates.follow;
            return;
        }

        rig.Rotate(Vector3.up, h * Time.deltaTime * rotSpeed);*/
    }

    public enum CameraStates
    {
        follow,
        orbit
    }
}

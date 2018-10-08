using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour {

    public float rotSpeed = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float h = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime * h);
		
	}
}

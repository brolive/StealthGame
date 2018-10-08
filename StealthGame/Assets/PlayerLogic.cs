using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLogic : MonoBehaviour {

    public float moveSpeed = 2.0f;
    public float groundCheckDistance = 1.0f;

    public PlayerState state = PlayerState.fall;

    public Vector3 moveDirection = new Vector3();

    public Camera mainCamera;

    public float stealthHealthMax = 100.0f;
    public float stealthHealthCurrent = 100.0f;

    Animator anim;
    CharacterController controller;

    public List<Vector3> timePos;

    public float recordElapsed = 0;
    public float rewindElapsed = 0;

    // Use this for initialization
    void Start () {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        bool slowTime = Input.GetMouseButton(1);
        bool rewind = Input.GetKey(KeyCode.Backspace) &&
                      stealthHealthCurrent > 2.0f;//Input.GetMouseButton(0);

        GameManager.instance.speed = (slowTime ? 0.25f : 1);

        stealthHealthCurrent -= (slowTime ? 1.0f : 0) * Time.deltaTime;
        UpdateStealthBar();

        anim.SetFloat("gameSpeed", GameManager.instance.speed);

        switch(state)
        {
            case PlayerState.fall:
                UpdateFall();
                break;
            case PlayerState.idle:
                UpdateIdle();
                break;
            case PlayerState.walk:
                UpdateWalk();
                break;
        }
        
        if(!rewind)
            controller.Move(moveDirection * Time.deltaTime * GameManager.instance.speed);
        
        if (rewind) // if rewinding
        {
            RewindPosition();
        }

        else // if not currently rewinding
        {
            RecordPosition();
            
        }

        recordElapsed += Time.deltaTime;
        rewindElapsed += Time.deltaTime;
    }

    void RecordPosition()
    {
        if (recordElapsed > 0.25f)
        {
            timePos.Add(transform.position); // add position to list
            if (timePos.Count > 1000)
            {
                timePos.RemoveAt(0);
            }

            recordElapsed = 0;
        }
    }

    void RewindPosition()
    {
        if (rewindElapsed > 0.05f)
        {
            if (timePos.Count > 0)
            {
                //transform.position = timePos[timePos.Count - 1];
                Vector3 pos = timePos[timePos.Count - 1];
                //transform.position = Vector3.Lerp(transform.position, timePos[timePos.Count - 1], Time.deltaTime * 5);

                transform.position = Vector3.Lerp(transform.position,
                                                  pos,
                                                  Time.deltaTime);

                timePos.RemoveAt(timePos.Count - 1);
                rewindElapsed = 0;
                stealthHealthCurrent -= 1.0f;
            }
        }
    }

    public void UpdateFall()
    {
        moveDirection.y -= GameManager.instance.gravity * Time.deltaTime;

        if (moveDirection.y <= -GameManager.instance.terminalVelocity)
        {
            moveDirection.y = -GameManager.instance.terminalVelocity;
        }

        if (IsGrounded())
        {
            moveDirection.y = 0;
            state = PlayerState.idle;
        }
    }

    public void UpdateIdle()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (h != 0 || v != 0)
        {
            anim.SetBool("sneak", true);
            anim.SetBool("idle", false);
            state = PlayerState.walk;
        }
    }

    public void UpdateWalk()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool sprint = Input.GetButton("Sprint");
        anim.SetBool("sprint", sprint);

        if (h == 0 && v == 0)
        {
            anim.SetBool("sneak", false);
            anim.SetBool("idle", true);
            state = PlayerState.idle;
            moveDirection *= 0;
            return;
        }
        
        // Update move direction
        // figure out correct directions based on camera angle
        Vector3 forward = mainCamera.transform.TransformDirection(Vector3.forward);
        forward.y = 0f;
        forward = forward.normalized;
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        // save off vertical speed
        float temp = moveDirection.y;
        // calculate move direction
        moveDirection = ((right * h) + (forward * v));

        // point toward move direction
        Vector3 newMoveDir = moveDirection;
        newMoveDir.y = 0;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                             Quaternion.LookRotation(newMoveDir * 10), 
                             Time.time * 0.75f * GameManager.instance.speed);

        moveDirection *= moveSpeed * (sprint ? 4 : 1);
    }

    public bool IsGrounded()
    {
        if(Physics.Raycast(transform.position, Vector3.down, groundCheckDistance))
        {
            Debug.Log("Grounded");

            return true;
        }

        return false;
    }

    void UpdateStealthBar()
    {
        GameObject.Find("StealthBar").GetComponent<Image>().fillAmount = stealthHealthCurrent / 100;
    }

    public enum PlayerState
    {
        fall,
        idle,
        walk
    }
}

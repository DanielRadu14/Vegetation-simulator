using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{   
    public List<Transform> opponents;
    public bool aiming;
    public bool engagingOpponent = false;
    public bool isWallRunning = false;
    public Vector3 wallProjectedMoveDir;

    void OnEnable()
    {
        opponents = new List<Transform>();
    }

    void Start()
    {
        GetCommonComponents();
    }
    
    void Update()
    {
        GetMoveDirection();
        base.FighterUpdate();
        HandleRun();
        IncrementTimers();
    }

    private void IncrementTimers()
    {
        timeSinceJump += Time.deltaTime;
    }

    private void HandleRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }
    
    private void GetMoveDirection()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveDir = (x * camera.right + z * camera.forward).normalized;
        moveDir = Vector3.ProjectOnPlane(moveDir, Vector3.up).normalized;
    }
}

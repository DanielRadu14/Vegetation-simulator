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
        FightStance();
        HandleAttack();
        HandleShooting();
        HandleWallRun();
        HandleSlide();
        HandleJump();
        IncrementTimers();
    }

    private void FightStance()
    {
        float minDistanceToOpponent = float.PositiveInfinity;
        foreach(Transform opponent in opponents)
        {
            Vector3 toOpponent = opponent.position - transform.position;
            float distToOpponent =  toOpponent.magnitude;
            if(distToOpponent < 2f && distToOpponent < minDistanceToOpponent)
            {
                minDistanceToOpponent = distToOpponent;
                engagingOpponent = true;
            }
            else
                engagingOpponent = false;
        }
        if(aiming)
        {
            //orientare player in directia aim-ului
            Vector3 lookDirection = moveDir;
            lookDirection = Vector3.ProjectOnPlane(camera.forward, Vector3.up).normalized;
            if(lookDirection.magnitude > 10e-3)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                    targetRotation, 
                                                    Time.deltaTime * rotSpeed);
            }
        }
        animator.SetFloat("distToOpponent", minDistanceToOpponent);
    }

    private void IncrementTimers()
    {
        timeSinceJump += Time.deltaTime;
    }

    private void HandleWallRun()
    {

        //minutul 51, tag stari, sa nu mai dea applyrootrotation la wallrun
        if (moveDir.magnitude < 10e-3f || !Input.GetButton("Jump"))
        {
            isWallRunning = false;
            animator.SetBool("WallRun", isWallRunning);
            return;
        }

        Ray ray = new Ray();
        ray.origin = transform.position + Vector3.up * capsule.height / 2f;
        ray.direction = moveDir;

        int layerMask = ~LayerMask.NameToLayer("WallRunnable");
        float wallRunMaxDistance = 1.5f;
        if (Physics.Raycast(ray, out RaycastHit hitInfo, wallRunMaxDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            isWallRunning = true;
			wallProjectedMoveDir = Vector3.ProjectOnPlane(moveDir, hitInfo.normal);
            if (Input.GetButtonDown("Jump"))
            {
                float moveDirCrossWallDirY = Vector3.Cross(moveDir, wallProjectedMoveDir).y;

                animator.SetBool("WallRunSide", moveDirCrossWallDirY < 0f);
                StartCoroutine(StopWallRunning(4f));
            }
        }
        else
        {
            isWallRunning = false;
        }
        animator.SetBool("WallRun", isWallRunning);
    }
    IEnumerator StopWallRunning(float t) 
    {
        yield return new WaitForSeconds(t);
        isWallRunning = false;
        animator.SetBool("WallRun", isWallRunning);
    }

    private void HandleSlide()
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

    private void HandleJump()
    {
        if(!Input.GetButtonDown("Jump") || isWallRunning)
            return;

        if(engagingOpponent)
            animator.Play("Roll");
        else if (grounded && timeSinceJump > 0.25f)
        {
            timeSinceJump = 0f;
            animator.Play("Takeoff");
        }
    }

    private void HandleAttack()
    {
        animator.SetBool("heavyPunch", Input.GetButton("Fire1"));
        if (!aiming && Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Punch");
            animator.SetBool("lightPunch", true);
            StartCoroutine(setLightPunchFalse());
        }
    }

    private IEnumerator setLightPunchFalse()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("lightPunch", false);
    }

    private void HandleShooting()
    {
        aiming = Input.GetButton("Fire2");
        animator.SetBool("Aiming", aiming);
    }
    
    private void GetMoveDirection()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveDir = (x * camera.right + z * camera.forward).normalized;
        moveDir = Vector3.ProjectOnPlane(moveDir, Vector3.up).normalized;
    }
}

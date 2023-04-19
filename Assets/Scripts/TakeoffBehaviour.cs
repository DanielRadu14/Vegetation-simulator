using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeoffBehaviour : StateMachineBehaviour
{
    bool hasToTakeoff = false;
    private Player player;
    private void OnEnable()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasToTakeoff = true;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime > 0.75 && hasToTakeoff)
        {
            DoJump();
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hasToTakeoff)
        {
            DoJump();
        }
    }

    private void DoJump()
    {
        Vector3 jumpDirection = (player.moveDir + Vector3.up * player.jumpUpPower).normalized;
        player.rigidbody.AddForce(jumpDirection * player.jumpPower, ForceMode.VelocityChange);
        hasToTakeoff = false;
    }
}

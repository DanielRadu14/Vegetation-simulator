using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHitBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       int newHP = animator.GetInteger("HP") - animator.GetInteger("takenDamage");
       animator.SetInteger("HP", newHP);
       if(newHP <= 0)
       {
           animator.Play("Die");
           TimeScaleManager.GetInstance().SetTimeScale(0.1f, 2.5f);
       }
    }
}

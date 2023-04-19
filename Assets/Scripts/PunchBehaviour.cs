using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchBehaviour : StateMachineBehaviour
{
    public float HitBoxStartTime = 0.044f;
    public float hitBoxEndTime = 0.2f;
    public HumanBodyBones bone;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var boneTransform = animator.GetBoneTransform(bone);
        var hitBox = boneTransform.GetComponent<HitBox>();
        hitBox.dirty = false;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var boneTransform = animator.GetBoneTransform(bone);
        var collider = boneTransform.GetComponent<Collider>();
        var hitBox = boneTransform.GetComponent<HitBox>();
        collider.enabled = stateInfo.normalizedTime > HitBoxStartTime &&
            stateInfo.normalizedTime < hitBoxEndTime && !hitBox.dirty;
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var boneTransform = animator.GetBoneTransform(bone);
        var collider = boneTransform.GetComponent<Collider>();
        collider.enabled = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

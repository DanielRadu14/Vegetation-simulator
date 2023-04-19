using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunCycleBehaviour : StateMachineBehaviour
{
    public float wallRunRotationOffset = 0f;
	public float wallRunSpeedMultiplier = 2f;
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Rigidbody rigidbody = animator.GetComponent<Rigidbody>();
        Player player = animator.GetComponent<Player>();
        Vector3 wallProjectedMoveDir = player.wallProjectedMoveDir.normalized;
        animator.transform.forward = Quaternion.Euler(0, wallRunRotationOffset, 0) * wallProjectedMoveDir;

        rigidbody.velocity = (animator.deltaPosition / Time.deltaTime).magnitude * wallProjectedMoveDir * wallRunSpeedMultiplier;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public string opponentLayer;
    public string side;
    private Collider collider;
    public bool dirty = false;
    public int damage = 5;

    void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(opponentLayer))
        {
            var animator = other.GetComponentInParent<Animator>();
            if(animator.GetBool("Alive") == false)
            {
                return;
            }
            animator.SetInteger("takenDamage", damage);
            animator.Play("TakeHit.TakeHit" + side);
            dirty = true;
            collider.enabled = false;
        }
    }
}

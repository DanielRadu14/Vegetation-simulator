using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //trailing proiectil, lab7, min 42
    private Animator animator;
    private Transform head;
    private Vector3 projectileDirection;
    float timeLived = 0f;
    float lifetime = 5f;
    public float speed = 10f;
    public int damage = 10;

    private void OnEnable()
    {
        timeLived = 0f;
    }
    
    void Start()
    {
        animator = GameObject.FindObjectOfType<Player>().GetComponent<Animator>();
        head = animator.GetBoneTransform(HumanBodyBones.Head);
    }

    void Update()
    {
        if(timeLived < 0.2f)
        {
            transform.position += speed * head.forward * Time.deltaTime;
            projectileDirection = head.forward;
        }
        else
            transform.position += speed * projectileDirection * Time.deltaTime;

        timeLived += Time.deltaTime;
        if(timeLived > lifetime)
        {
            gameObject.SetActive(false);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            var enemyAnimator = other.GetComponent<Animator>();
            enemyAnimator.SetInteger("takenDamage", damage);
            enemyAnimator.Play(Random.Range(0,2) == 0 ? "TakeHit.TakeHitLeft" : "TakeHit.TakeHitRight");
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingMechanics : MonoBehaviour
{
    public Transform weapon;
    public Transform weaponTip;
    public Transform projectilesContainer;
    private Transform rightHand, head;
    private Animator animator;
    private Player player;
    private GameObject[] projectiles;
    
    private GameObject projectile;
    private bool launched = false;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
        rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
        head = animator.GetBoneTransform(HumanBodyBones.Head);
        projectiles = new GameObject[projectilesContainer.childCount];
        for(int i = 0; i < projectilesContainer.childCount; i++)
        {
            projectiles[i] = projectilesContainer.GetChild(i).gameObject;
        }
    }

    void LateUpdate()
    {
        weapon.gameObject.SetActive(player.aiming);
        if(player.aiming)
        {
            head.rotation = player.camera.rotation;
            CopyRightHandTransformToWeapon();
            if(Input.GetButtonDown("Fire1"))
            {
                for(int i = 0; i < projectilesContainer.childCount; i++)
                {
                    if(!projectiles[i].activeInHierarchy)
                    {
                        if(launched == false)
                        {
                            animator.Play("Throw");
                            projectile = projectiles[i];
                            launched = true;
                            StartCoroutine(LaunchProjectile());
                        }
                        break;
                    }
                }
            }
        }
    }

    private IEnumerator LaunchProjectile()
    {
        yield return new WaitForSeconds(0.5f);
        projectile.SetActive(true);
        projectile.transform.position = weapon.position;
        projectile.transform.rotation = weapon.rotation;
        yield return new WaitForSeconds(1.5f);
        launched = false;
    }

    private void CopyRightHandTransformToWeapon()
    {
        weapon.position = rightHand.position;
        weapon.rotation = rightHand.rotation;
    }
}

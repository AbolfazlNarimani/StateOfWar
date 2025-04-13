using System;
using NewInputSystem.ActionSystem.MoveAction;
using NewInputSystem.ActionSystem.ShootAction;
using Unity.VisualScripting;
using UnityEngine;

namespace UnitAnimator
{
    public class UnitAnimator : MonoBehaviour
    {
        private const string IsMoving = "IsMoving";
        private const string Shooting = "Shoot";

     [SerializeField] private Animator animator;
     [SerializeField] private Transform bulletProjectilePrefab;
     [SerializeField] private Transform bulletSpawnPoint;

     private void Awake()
     {
         if (TryGetComponent<MoveAction>(out MoveAction moveAction))
         {
             moveAction.OnStartMoving += (sender, args) => animator.SetBool(IsMoving, true); 
             moveAction.OnStopMoving += (sender, args) => animator.SetBool(IsMoving, false);
         }

         if (TryGetComponent(out ShootAction shootAction))
         {
             shootAction.OnUnitShoot += ShootActionOnUnitShoot;
         }
     }

     private void ShootActionOnUnitShoot(object sender, ShootAction.OnShootEventArgs e)
     {
         animator.SetTrigger(Shooting);
         Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab,bulletSpawnPoint.position, Quaternion.identity );
         BulletProjectile.BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile.BulletProjectile>();
         Vector3 targetUnitShootAtPosition = e.TargetUnit.GetWorldPosition();
         targetUnitShootAtPosition.y = bulletProjectileTransform.position.y;
         bulletProjectile.Setup(targetUnitShootAtPosition);
     }
    }
}








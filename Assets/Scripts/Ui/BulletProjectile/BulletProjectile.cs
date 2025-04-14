using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

namespace BulletProjectile
{
    public class BulletProjectile : MonoBehaviour
    {
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private Transform bulletHitVfxPrefab;
        private Vector3 _targetPosition;
        public void Setup(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
        }

        private void Update()
        {
            float distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);
            
            Vector3 moveDir = (_targetPosition - transform.position).normalized;
            float moveSpeed = 200f;
            transform.position += moveDir * (moveSpeed * Time.deltaTime);
            
            float distanceAfterMoving = Vector3.Distance(transform.position, _targetPosition);

            if (distanceBeforeMoving < distanceAfterMoving)
            {
                transform.position = _targetPosition;
                trailRenderer.transform.parent = null;
                Destroy(gameObject);
                Instantiate(bulletHitVfxPrefab,_targetPosition,Quaternion.identity);
            }
        }
    }
}

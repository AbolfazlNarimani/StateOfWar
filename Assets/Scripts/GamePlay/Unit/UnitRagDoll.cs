using Unity.VisualScripting;
using UnityEngine;

namespace GamePlay.Unit
{
    public class UnitRagDoll : MonoBehaviour
    {
        [SerializeField] private Transform ragDollRootBone;

        public void SetUp(Transform originalRagDollRootBone)
        {
            MatchAllChildTransforms(originalRagDollRootBone, ragDollRootBone);
            //optional
             ApplyExplosionToRagdoll(ragDollRootBone, .5f, 1f);
             
        }

        private void MatchAllChildTransforms(Transform root, Transform clone)
        {
            foreach (Transform child in root)
            {
                Transform cloneChild = clone.Find(child.name);
                if (cloneChild != null)
                {
                    cloneChild.position = child.position;
                    cloneChild.rotation = child.rotation;

                    MatchAllChildTransforms(child, cloneChild);
                   
                }
            }
        }

        private void ApplyExplosionToRagdoll(Transform root, float explosionForce,float explosionRange)
        {
            void OnTriggerEnter(Collider other)
            {
                foreach (Transform child in root)
                {
                    if (child.TryGetComponent(out Rigidbody childRagdollBody))
                    {
                        childRagdollBody.AddExplosionForce(explosionForce, other.transform.position, explosionRange);
                    }
                    ApplyExplosionToRagdoll(child, explosionForce, explosionRange);
                }
            }

            
        }
    }
}
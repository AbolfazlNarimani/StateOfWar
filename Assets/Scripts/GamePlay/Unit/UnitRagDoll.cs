using UnityEngine;

namespace GamePlay.Unit
{
    public class UnitRagDoll : MonoBehaviour
    {
        [SerializeField] private Transform ragDollRootBone;

        public void SetUp(Transform originalRagDollRootBone)
        {
            MatchAllChildTransforms(originalRagDollRootBone, ragDollRootBone);
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
    }
}
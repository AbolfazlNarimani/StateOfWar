using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay.GridSystem
{
    public class Testing : MonoBehaviour
    {
        [FormerlySerializedAs("_unit")] [SerializeField] private GamePlay.Unit.Unit unit;
        private void Update()
        {

        }
    }
}
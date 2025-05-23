using TMPro;
using UnityEngine;

namespace GamePlay.GridSystem
{
    public class GridDebugObject : MonoBehaviour
    {
        private object _gridObject;
        [SerializeField] private TextMeshPro textMeshPro;

        public virtual void SetGridObject(object gridObject)
        {
            this._gridObject = gridObject;
        }

        protected virtual void Update()
        {
            textMeshPro.text = this._gridObject.ToString();
        }
        
    }
}
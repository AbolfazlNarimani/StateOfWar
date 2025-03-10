using System;
using TMPro;
using UnityEngine;

namespace GridSystem
{
    public class GridDebugObject : MonoBehaviour
    {
        private GridObject _gridObject;
        [SerializeField] private TextMeshPro textMeshPro;

        public void SetGridObject(GridObject gridObject)
        {
            this._gridObject = gridObject;
        }

        private void Update()
        {
            textMeshPro.text = this._gridObject.ToString();
        }
        
    }
}
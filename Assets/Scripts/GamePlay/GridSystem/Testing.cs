using System.Collections.Generic;
using GridSystem;
using NewInputSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay.GridSystem
{
    public class Testing : MonoBehaviour
    {
        [FormerlySerializedAs("_unit")] [SerializeField] private Unit.InfantryUnit.Unit unit;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMouseWorldPosition());
                
                GridPosition startGridPosition = new GridPosition(0,0);
                
                List<GridPosition> gridPositionList = PathFinding.PathFinding.Instance.FindPath(startGridPosition, mouseGridPosition);
                
                for (int i = 0; i < gridPositionList.Count - 1; i++)
                {
                    Debug.DrawLine(LevelGrid.Instance.GetWorldPosition(gridPositionList[i]), LevelGrid.Instance.GetWorldPosition(gridPositionList[i+1]), Color.green, 10f);
                }
            }
        }
    }
}
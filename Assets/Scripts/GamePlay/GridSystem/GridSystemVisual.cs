using System;
using System.Collections.Generic;
using GamePlay.ActionSystem.BaseAction;
using GamePlay.ActionSystem.BaseShootAction.ShootAction;
using GamePlay.ActionSystem.MoveAction;
using GamePlay.ActionSystem.SpinAction;
using GamePlay.Health;
using GamePlay.Unit;
using GamePlay.Unit.BaseUnit;
using GridSystem;
using UnityEngine;

namespace GamePlay.GridSystem
{
    public class GridSystemVisual : MonoBehaviour
    {
        public static GridSystemVisual Instance { private set; get; }
        [Serializable]
        public struct GridVisualTypeMaterial
        {
            public GridVisualType gridVisualType;
            public Material material;
        }
        [SerializeField]private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;

        public enum GridVisualType
        {
            White,
            Blue,
            TargetRed,
            RedSoft,
            Yellow
        }

        [SerializeField] private Transform gridSystemVisualSinglePrefab;
        private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _gridSystemVisualSingleArray =
                new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];

            for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            {
                for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab,
                        LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                    _gridSystemVisualSingleArray[x, z] =
                        gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
                }
            }

            UnitActionSystem.Instance.OnSelectedActionChanged += InstanceOnSelectedActionChanged;
            LevelGrid.Instance.OnAnyUnitMovedGridPosition += InstanceOnAnyUnitMovedGridPosition;
            LevelGrid.Instance.OnAnyUnitDied += InstanceOnAnyUnitDied;

            UpdateGridSystemVisual();
        }

        private void InstanceOnAnyUnitDied(object sender, EventArgs e)
        {
            if (UnitActionSystem.Instance == null) return;
            if (LevelGrid.Instance == null) return;

            BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
            if (selectedAction == null) return;

            UpdateGridSystemVisual();
        }

        private void InstanceOnAnyUnitMovedGridPosition(object sender, EventArgs e)
        {
            if (UnitActionSystem.Instance == null) return;
            if (LevelGrid.Instance == null) return;

            BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
            if (selectedAction == null) return;

            UpdateGridSystemVisual();
        }

        private void InstanceOnSelectedActionChanged(object sender, EventArgs e)
        {
            if (UnitActionSystem.Instance == null) return;
            if (LevelGrid.Instance == null) return;

            BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
            if (selectedAction == null) return;

            UpdateGridSystemVisual();
        }

        private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
        {
            foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
            {
                if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
                {
                    return gridVisualTypeMaterial.material;
                }
            }

            Debug.LogError("could not find GridVisualType in GetGridVisualType");
            return null;
        }


        public void HideAllGridPositions()
        {
            for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            {
                for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
                {
                    _gridSystemVisualSingleArray[x, z].Hide();
                }
            }
        }

        public void ShowAllGridPositions(List<GridPosition> gridPositionsList, GridVisualType gridVisualType)
        {
            foreach (GridPosition gridPosition in gridPositionsList)
            {
                _gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
            }
        }

        private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
        {
            List<GridPosition> gridPositionList = new List<GridPosition>();
            for (int x = -range; x <= range; x++)
            {
                for (int z = -range; z <= range; z++)
                {
                    
                    GridPosition testGridPosition = gridPosition + new GridPosition(x,z);
                    if (!LevelGrid.Instance.IsGridPositionValid(testGridPosition))
                    {
                        continue;
                    }
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > range)
                    {
                        continue;
                    }
                    gridPositionList.Add(testGridPosition);
                  
                }
            }

            
            ShowAllGridPositions(gridPositionList, gridVisualType);
        }

        private void UpdateGridSystemVisual()
        {
            BaseUnit selectedUnit = UnitActionSystem.Instance?.GetSelectedUnit();
            if (selectedUnit == null) return;

            BaseAction selectedAction = UnitActionSystem.Instance?.GetSelectedAction();
            if (selectedAction == null) return;

            HideAllGridPositions();
            GridVisualType gridVisualType = GridVisualType.White;
            switch (selectedAction)
            {
                case MoveAction moveAction:
                    gridVisualType = GridVisualType.White;
                    break;
                case ShootAction shootAction:
                    gridVisualType = GridVisualType.TargetRed;
                    ShowGridPositionRange(selectedUnit.GetGridPosition(),shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                    break;
                case SpinAction spinAction:
                    gridVisualType = GridVisualType.Blue;
                    break;
            }
            Instance.ShowAllGridPositions(selectedAction.GetValidActionGridPositionList(),gridVisualType);
        }
    }
}
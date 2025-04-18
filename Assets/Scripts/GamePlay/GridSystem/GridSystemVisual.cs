using System.Collections.Generic;
using GamePlay.ActionSystem.BaseAction;
using GamePlay.Unit;
using GamePlay.Unit.BaseUnit;
using GridSystem;
using UnityEngine;

namespace GamePlay.GridSystem
{
    public class GridSystemVisual : MonoBehaviour
    {
        public static GridSystemVisual Instance { private set; get; }

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
        }

        private void Update()
        {
            if (UnitActionSystem.Instance == null) return;
            if (LevelGrid.Instance == null) return;

            BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
            if (selectedAction == null) return;

            UpdateGridSystemVisual();
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

        public void ShowAllGridPositions(List<GridPosition> gridPositionsList)
        {
            foreach (GridPosition gridPosition in gridPositionsList)
            {
                _gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
            }
        }

        private void UpdateGridSystemVisual()
        {
            BaseUnit selectedUnit = UnitActionSystem.Instance?.GetSelectedUnit();
            if (selectedUnit == null) return;

            BaseAction selectedAction = UnitActionSystem.Instance?.GetSelectedAction();
            if (selectedAction == null) return;

            HideAllGridPositions();
            Instance.ShowAllGridPositions(selectedAction.GetValidActionGridPositionList());
        }
    }
}
using System;
using System.Collections.Generic;
using NewInputSystem.ActionSystem.BaseAction;
using Unit;
using UnityEngine;

namespace GridSystem
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
            HideAllGridPositions();
            BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
            
            Instance.ShowAllGridPositions(selectedAction.GetValidActionGridPositionList());
        }

        private void Update()
        {
            UpdateGridSystemVisual();
        }
    }
}
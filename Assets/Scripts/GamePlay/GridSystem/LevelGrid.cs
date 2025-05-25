using System.Collections.Generic;
using GridSystem;
using UnityEngine;
using System;


namespace GamePlay.GridSystem
{
    public class LevelGrid : MonoBehaviour
    {
        public static LevelGrid Instance;
        [SerializeField] Transform debugObjectPrefab;
        private GridSystem<GridObject> _gridSystem;

        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private float cellSize;

        public event EventHandler OnAnyUnitMovedGridPosition;
        public event EventHandler OnAnyUnitDied;

        private void Awake()
        {
            Instance = this;
            _gridSystem = new GamePlay.GridSystem.GridSystem<GridObject>(width, height, cellSize,
                (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
            //_gridSystem.CreateDebugObjects(debugObjectPrefab);
        }

        private void Start()
        {
            PathFinding.PathFinding.Instance.SetUp(width, height, cellSize);
        }

        public void AddUnitAtGridPosition(GridPosition gridPosition, GamePlay.Unit.BaseUnit.BaseUnit unit)
        {
            GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            gridObject.AddUnit(unit);
        }

        public List<GamePlay.Unit.BaseUnit.BaseUnit> GetUnitListAtGridPosition(GridPosition gridPosition)
        {
            GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            return gridObject.GetUnitList();
        }

        public void RemoveUnitAtGridPosition(GridPosition gridPosition, GamePlay.Unit.BaseUnit.BaseUnit unit)
        {
            GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            OnAnyUnitDied?.Invoke(this, EventArgs.Empty);
            gridObject.RemoveUnit(unit);
        }

        public void UnitMovedGridPosition(GamePlay.Unit.BaseUnit.BaseUnit unit, GridPosition fromGridPosition,
            GridPosition toGridPosition)
        {
            RemoveUnitAtGridPosition(fromGridPosition, unit);
            AddUnitAtGridPosition(toGridPosition, unit);
            OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
        }

        public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);
        public bool IsGridPositionValid(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);

        public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);

        public int GetWidth() => _gridSystem.GetWidth();
        public int GetHeight() => _gridSystem.GetHeight();

        public bool HasAnyUnitAtGridPosition(GridPosition gridPosition)
        {
            GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            return gridObject.ContainsUnit();
        }

        public GamePlay.Unit.BaseUnit.BaseUnit GetUnitAtGridPosition(GridPosition gridPosition)
        {
            GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            return gridObject.GetUnit();
        }
    }
}
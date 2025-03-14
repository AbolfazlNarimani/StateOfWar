using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

namespace GridSystem
{
    public class LevelGrid : MonoBehaviour
    {
        public static LevelGrid Instance;
        [SerializeField] Transform debugObjectPrefab;
        private GridSystem _gridSystem;

        private void Awake()
        {
            Instance = this;
            _gridSystem = new GridSystem(10, 10, 2f);
            _gridSystem.CreateDebugObjects(debugObjectPrefab);
        }

        public void AddUnitAtGridPosition(GridPosition gridPosition, Unit.Unit unit)
        {
            GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            gridObject.AddUnit(unit);
        }

        public List<Unit.Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
        {
            GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            return gridObject.GetUnitList();
        }

        public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit.Unit unit)
        {
            GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            gridObject.RemoveUnit(unit);
        }

        public void UnitMovedGridPosition(Unit.Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
        {
            RemoveUnitAtGridPosition(fromGridPosition, unit);
            AddUnitAtGridPosition(toGridPosition, unit);
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
    }
}
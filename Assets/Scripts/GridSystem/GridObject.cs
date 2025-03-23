using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class GridObject
    {
        private GridPosition _gridPosition;
        private List<Unit.Unit> _unitList;
        private GridSystem _gridSystem;

        public GridObject(GridSystem gridSystem, GridPosition gridPosition)
        {
            this._gridSystem = gridSystem;
            this._gridPosition = gridPosition;
            _unitList = new List<Unit.Unit>();
        }

        public override string ToString()
        {
            string unitString = "";
            foreach (var unit in _unitList)
            {
                unitString += unit + "\n";
            }
            return _gridPosition.ToString() + "\n" + unitString;
        }

        public void AddUnit(Unit.Unit unit)
        {
            _unitList.Add(unit);
        }

        public List<Unit.Unit> GetUnitList()
        {
            return _unitList;
        }

        public void RemoveUnit(Unit.Unit unit)
        {
            _unitList.Remove(unit);
        }

        public bool ContainsUnit()
        {
            return _unitList.Count > 0;
        }

        public Unit.Unit GetUnit()
        {
            if (ContainsUnit())
            {
                return _unitList[0];
            }
            return null;
        }
    }
}
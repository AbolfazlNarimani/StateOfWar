using System.Collections.Generic;
using GamePlay.Unit.BaseUnit;
using GridSystem;

namespace GamePlay.GridSystem
{
    public class GridObject
    {
        private GridPosition _gridPosition;
        private List<GamePlay.Unit.BaseUnit.BaseUnit> _unitList;
        private GamePlay.GridSystem.GridSystem<GridObject> _gridSystem;

        public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
        {
            _gridSystem = gridSystem;
            _gridPosition = gridPosition;
            _unitList = new List<BaseUnit>();
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

        public void AddUnit(GamePlay.Unit.BaseUnit.BaseUnit unit)
        {
            _unitList.Add(unit);
        }

        public List<GamePlay.Unit.BaseUnit.BaseUnit> GetUnitList()
        {
            return _unitList;
        }

        public void RemoveUnit(GamePlay.Unit.BaseUnit.BaseUnit unit)
        {
            _unitList.Remove(unit);
        }

        public bool ContainsUnit()
        {
            return _unitList.Count > 0;
        }

        public GamePlay.Unit.BaseUnit.BaseUnit GetUnit()
        {
            if (ContainsUnit())
            {
                return _unitList[0];
            }
            return null;
        }
    }
}
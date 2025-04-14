using System.Collections.Generic;
using GridSystem;

namespace GamePlay.GridSystem
{
    public class GridObject
    {
        private GridPosition _gridPosition;
        private List<GamePlay.Unit.Unit> _unitList;
        private GamePlay.GridSystem.GridSystem _gridSystem;

        public GridObject(GamePlay.GridSystem.GridSystem gridSystem, GridPosition gridPosition)
        {
            this._gridSystem = gridSystem;
            this._gridPosition = gridPosition;
            _unitList = new List<GamePlay.Unit.Unit>();
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

        public void AddUnit(GamePlay.Unit.Unit unit)
        {
            _unitList.Add(unit);
        }

        public List<GamePlay.Unit.Unit> GetUnitList()
        {
            return _unitList;
        }

        public void RemoveUnit(GamePlay.Unit.Unit unit)
        {
            _unitList.Remove(unit);
        }

        public bool ContainsUnit()
        {
            return _unitList.Count > 0;
        }

        public GamePlay.Unit.Unit GetUnit()
        {
            if (ContainsUnit())
            {
                return _unitList[0];
            }
            return null;
        }
    }
}
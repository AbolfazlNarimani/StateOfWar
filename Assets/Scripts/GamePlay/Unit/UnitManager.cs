using System;
using System.Collections.Generic;
using GamePlay.Unit.BaseUnit;
using UnityEngine;

namespace GamePlay.Unit
{
    public class UnitManager : MonoBehaviour
    {
        private List<BaseUnit.BaseUnit> unitList;
        private List<BaseUnit.BaseUnit> friendlyUnitList;
        private List<BaseUnit.BaseUnit> enemyUnitList;

        
        public static UnitManager Instance{private set; get;}

        private void Awake()
        {
            Instance = this;
            unitList = new List<BaseUnit.BaseUnit>();
            friendlyUnitList = new List<BaseUnit.BaseUnit>();
            enemyUnitList = new List<BaseUnit.BaseUnit>();
        }

        private void Start()
        {
            BaseUnit.BaseUnit.OnAnyUnitSpawned += BaseUnitOnAnyUnitSpawned;
            BaseUnit.BaseUnit.OnAnyUnitDead += BaseUnitOnAnyUnitDead;
        }

        private void BaseUnitOnAnyUnitDead(object sender, EventArgs e)
        {
            BaseUnit.BaseUnit unit = sender as BaseUnit.BaseUnit;
            if (unit.IsEnemy())
            {
                enemyUnitList.Remove(unit);
              
            }
            else
            {
                friendlyUnitList.Remove(unit);
               
            }
            unitList.Remove(unit);
          
        }

        private void BaseUnitOnAnyUnitSpawned(object sender, EventArgs e)
        {
            BaseUnit.BaseUnit unit = sender as BaseUnit.BaseUnit;
            if (unit.IsEnemy())
            {
                enemyUnitList.Add(unit);
            }
            else
            {
                friendlyUnitList.Add(unit);
            }
            unitList.Add(unit);
        }

        public List<BaseUnit.BaseUnit> GetUnitList()
        {
            return unitList;
        }
        public List<BaseUnit.BaseUnit> GetFriendlyUnitList()
        {
            return friendlyUnitList;
        }
        public List<BaseUnit.BaseUnit> GetEnemyUnitList()
        {
            return enemyUnitList;
        }
    }
}

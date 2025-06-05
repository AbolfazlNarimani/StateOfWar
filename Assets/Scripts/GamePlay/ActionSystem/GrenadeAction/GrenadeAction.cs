using System;
using System.Collections.Generic;
using GamePlay.Enemy.EnemyAI;
using GridSystem;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

namespace GamePlay.ActionSystem.GrenadeAction
{
    public class GrenadeAction : BaseAction.BaseAction
    {
        [SerializeField] private Sprite grenadeActionIcon;
        [SerializeField] private Vector2 customSpriteSize;

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }
            ActionComplete();
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            GridPosition unitGridPosition = Unit.GetGridPosition();
            return new List<GridPosition> { unitGridPosition };
        }

        public override string GetActionName()
        {
            return "Grenade";
        }

        public override Sprite GetActionIcon()
        {
         //   RectTransform rectTransform = grenadeActionIcon.GetComponent<RectTransform>();
         //   rectTransform.sizeDelta = customSpriteSize;
            return grenadeActionIcon;
        }

        public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
        {
            ActionStart(OnActionComplete);
        }

        public override int GetActionPointsCost()
        {
            return 2;
        }

        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            return new EnemyAIAction()
            {
                gridPosition = gridPosition,
                actionValue = 0
            };
        }

        public override int GetActionNameFontSize()
        {
            return 14;
        }

       
    }
}

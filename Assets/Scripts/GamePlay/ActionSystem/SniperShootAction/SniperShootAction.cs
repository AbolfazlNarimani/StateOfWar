using System;
using System.Collections.Generic;
using GamePlay.ActionSystem.BaseShootAction.ShootAction;
using GamePlay.GridSystem;
using GridSystem;
using UnityEngine;

namespace GamePlay.ActionSystem.SniperShootAction
{
    public class SniperShootAction : ShootAction
    {
        [Header("Sniper Specific Settings")]
        [SerializeField] private int sniperRange = 7;
        [SerializeField] private int sniperDamage = 75;
        [SerializeField] private int sniperActionCost = 2;
        [SerializeField] private bool requiresLineOfSight = true;
    
        protected override void Awake()
        {
            base.Awake();
            // Override base values with sniper-specific values
            maxShootDistance = sniperRange;
            damageAmount = sniperDamage;
            actionPointCost = sniperActionCost;
            SetActionNameForChildActions("Snipe");
        }
    
        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validPositions = base.GetValidActionGridPositionList();
        
            if (requiresLineOfSight)
            {
                // Filter out positions without line of sight
                validPositions.RemoveAll(pos => !HasLineOfSight(pos));
            }
        
            return validPositions;
        }
    
        private bool HasLineOfSight(GridPosition targetPosition)
        {
            // Implement your line of sight checking logic here
            // Could use raycasting or Bresenham's line algorithm
            Vector3 start = Unit.GetWorldPosition();
            Vector3 end = LevelGrid.Instance.GetWorldPosition(targetPosition);
        
            // Simple version - adjust for your game's needs
            return !Physics.Linecast(start, end, out RaycastHit hit, 
                LayerMask.GetMask("Obstacles"));
        }
    
        public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
        {
            // Sniper might have special effects
            PlaySniperVisualEffects();
            base.TakeAction(gridPosition, OnActionComplete);
        }
    
        private void PlaySniperVisualEffects()
        {
            // Implement sniper-specific visual/audio effects
        }
    }
}

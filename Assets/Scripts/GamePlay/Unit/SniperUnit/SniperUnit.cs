using GamePlay.ActionSystem.BaseAction;
using GamePlay.ActionSystem.BaseShootAction.ShootAction;
using UnityEngine;

namespace GamePlay.Unit.SniperUnit
{
    public class SniperUnit : BaseUnit.BaseUnit
    {
        private ShootAction _shootAction;

        //dfine the shoot range in the sniping action not here
        [SerializeField] private int shootRange = 5; // Default range, can be adjusted in inspector

        protected override void Awake()
        {
            base.Awake();
            _shootAction = GetComponent<ShootAction>();
        }

        public ShootAction GetShootAction() => _shootAction;
        public int GetShootRange() => shootRange;

        // Override any methods that need different behavior for sniper
        // For example, if sniper has different action point costs:
        public override bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
        {
            // Sniper might have different costs for certain actions
            if (baseAction is ShootAction)
            {
                // Maybe snipers spend more points for shooting define in sniping action
                if (actionPoints >= baseAction.GetActionPointsCost())
                {
                    SpendActionPoints(baseAction.GetActionPointsCost());
                    return true;
                }

                return false;
            }

            return base.TrySpendActionPointsToTakeAction(baseAction);
        }
    }
}
using GamePlay.ActionSystem.SpinAction;

namespace GamePlay.Unit.InfantryUnit
{
    public class Unit : BaseUnit.BaseUnit
    {
        private SpinAction _spinAction;

        protected override void Awake()
        {
            base.Awake();
            _spinAction = GetComponent<SpinAction>();
        }

        public SpinAction GetSpinAction() => _spinAction;
        
        
    }
}
. using Godot;
using System;

namespace StateMachine.States
{
    public class AlertState : BaseState
    {
        public AlertState(Guard guard)
            : base(guard, "Alert")
        {
        }

        public override void Enter()
        {
            base.Enter();

            // Initialize the sub-state machine if it doesn't exist
            if (SubStateMachine == null)
            {
                SubStateMachine = new StateMachine(ParentStateMachine, this);
                SubStateMachine.AddState(new PlayerDetectedState(_guard));
                SubStateMachine.AddState(new PlayerLostState(_guard));
                SubStateMachine.SetState<PlayerDetectedState>();
            }
        }

        public override void Update(float delta)
        {
            base.Update(delta);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;

            // Allow transitions to PatrolState
            return targetState is PatrolState;
        }
    }
}

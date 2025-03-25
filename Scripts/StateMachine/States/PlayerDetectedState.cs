using Godot;
using System;

namespace StateMachine.States
{
    public class PlayerDetectedState : BaseState
    {
        public PlayerDetectedState(Guard guard)
            : base(guard, "PlayerDetected")
        {
        }

        public override void Enter()
        {
            base.Enter();

            // Initialize the sub-state machine if it doesn't exist
            if (SubStateMachine == null)
            {
                SubStateMachine = new StateMachine(ParentStateMachine, this);
                SubStateMachine.AddState(new ChaseState(_guard));
                SubStateMachine.AddState(new AttackState(_guard));
                SubStateMachine.SetState<ChaseState>();
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

            // Allow transitions to PlayerLostState
            return targetState is PlayerLostState;
        }
    }
}

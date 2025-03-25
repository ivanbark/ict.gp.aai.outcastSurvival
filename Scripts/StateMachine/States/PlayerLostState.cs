using Godot;
using System;

namespace StateMachine.States
{
    public class PlayerLostState : BaseState
    {
        public PlayerLostState(Guard guard)
            : base(guard, "PlayerLost")
        {
        }

        public override void Enter()
        {
            base.Enter();

            // Initialize the sub-state machine if it doesn't exist
            if (SubStateMachine == null)
            {
                SubStateMachine = new StateMachine(ParentStateMachine, this);
                SubStateMachine.AddState(new SeekState(_guard));
                SubStateMachine.AddState(new SearchState(_guard));
                SubStateMachine.SetState<SeekState>();
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

            // Allow transitions to PlayerDetectedState or PatrolState
            return targetState is PlayerDetectedState || targetState is PatrolState;
        }
    }
}

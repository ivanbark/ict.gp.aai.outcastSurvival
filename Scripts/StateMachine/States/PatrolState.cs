using Godot;
using System;

namespace StateMachine.States
{
    public class PatrolState : BaseState
    {
        public PatrolState(Guard guard)
            : base(guard, "Patrol")
        {
        }

        public override void Enter()
        {
            base.Enter();

            // Initialize the sub-state machine if it doesn't exist
            if (SubStateMachine == null)
            {
                SubStateMachine = new StateMachine(ParentStateMachine, this);
                SubStateMachine.AddState(new IdleState(_guard));
                SubStateMachine.AddState(new PatrolPathState(_guard));
                SubStateMachine.SetState<IdleState>();
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

            // Only allow transitions to PatrolPathState or AlertState
            return targetState is PatrolPathState || targetState is AlertState;
        }
    }
}

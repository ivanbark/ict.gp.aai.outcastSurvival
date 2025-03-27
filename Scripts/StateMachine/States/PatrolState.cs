using Godot;
using OutCastSurvival.Entities;
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

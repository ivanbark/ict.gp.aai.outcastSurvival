using Godot;
using OutCastSurvival.Entities;
using System;

namespace StateMachine.States
{
    public class AlertState : BaseState
    {
        public AlertState(Guard guard, Node2D parent)
            : base(guard, "Alert", parent)
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

            // Allow transitions to PatrolState
            return targetState is PatrolState;
        }
    }
}

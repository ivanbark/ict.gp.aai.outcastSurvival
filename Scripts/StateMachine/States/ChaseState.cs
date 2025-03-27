using Godot;
using OutCastSurvival.Entities;
using System;

namespace StateMachine.States
{
    public class ChaseState : BaseState
    {
        public ChaseState(Guard guard)
            : base(guard, "Chase")
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update(float delta)
        {
            GD.Print("ChaseState Update");
            if (!IsActive || _guard == null || _guard.Player == null) return;

            _guard.ChasePlayer(delta);

            if (_guard.Position.DistanceTo(_guard.Player.Position) <= _guard.AttackRange - 5)
            {
                if (ParentStateMachine != null)
                {
                    TransitionTo<AttackState>();
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;

            // Allow transitions to AttackState or PlayerLostState
            return targetState is AttackState || targetState is PlayerLostState;
        }
    }
}

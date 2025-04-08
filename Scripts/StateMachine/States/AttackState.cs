using Godot;
using OutCastSurvival.Entities;
using System;

namespace StateMachine.States
{
    public class AttackState : BaseState
    {
        private float _attackSpeedModifier = 0.8f;
        private int _previousMaxSpeed;
        public AttackState(Guard guard, Node2D parent)
            : base(guard, "Attack", parent)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _previousMaxSpeed = _guard.MaxSpeed;

            _guard.MaxSpeed = (int)Math.Round(_guard.MaxSpeed * _attackSpeedModifier);

            _guard.IsAttacking = true;
        }

        public override void Update(float delta)
        {
            if (!IsActive || _guard == null || _guard.Player == null) return;

            _guard.AttackPlayer(delta);

            if (_guard.Player.Velocity.Length() <= 0 && _guard.Position.DistanceTo(_guard.Player.Position) > 5)
            {
                Vector2 desiredVelocity = SteeringBehaviour.Arrive(_guard.Position, _guard.Player.Position, _guard.MaxSpeed, 30);
                _guard.ApplyAcceleration(desiredVelocity, delta);
            }

            if (_guard.Position.DistanceTo(_guard.Player.Position) > _guard.AttackRange)
            {
                TransitionTo<ChaseState>();
            }
        }

        public override void Exit()
        {
            base.Exit();
            _guard.MaxSpeed = _previousMaxSpeed;
            _guard.IsAttacking = false;
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;

            return targetState is ChaseState || targetState is PlayerLostState;
        }
    }
}

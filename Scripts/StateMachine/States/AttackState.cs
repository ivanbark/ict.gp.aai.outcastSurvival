using Godot;
using System;

namespace StateMachine.States
{
    public class AttackState : BaseState
    {
        public AttackState(Guard guard, Node2D parent)
            : base(guard, "Attack", parent)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _guard.Velocity = Vector2.Zero;

            _guard.animatedSprite.Animation = "attack";
            _guard.animatedSprite.Play();
        }

        public override void Update(float delta)
        {
            if (!IsActive || _guard == null || _guard.Player == null) return;

            _guard.AttackPlayer(delta);

            if (_guard.Position.DistanceTo(_guard.Player.Position) > _guard.AttackRange)
            {
                TransitionTo<ChaseState>();
            }
        }

        public override void Exit()
        {
            base.Exit();
            _guard.animatedSprite.Stop();
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;

            // Only allow transitions to ChaseState or PlayerLostState
            return targetState is ChaseState || targetState is PlayerLostState;
        }
    }
}

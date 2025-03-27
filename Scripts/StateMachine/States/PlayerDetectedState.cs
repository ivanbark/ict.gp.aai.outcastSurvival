using Godot;
using System;

namespace StateMachine.States
{
    public class PlayerDetectedState : BaseState
    {
        public PlayerDetectedState(Guard guard, Node2D parent)
            : base(guard, "PlayerDetected", parent)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            if (_guard.Position.DistanceTo(_guard.Player.Position) > 100)
            {
                TransitionTo<PlayerLostState>();
            }
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

using Godot;
using OutCastSurvival.Entities;
using System;

namespace StateMachine.States
{
    public class PlayerLostState : BaseState
    {
        public PlayerLostState(Guard guard, Node2D parent)
            : base(guard, "PlayerLost", parent)
        {
        }

        public override void Enter()
        {
            base.Enter();

            TransitionToChild<SeekState>();
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

            return targetState is PlayerDetectedState || targetState is PatrolState;
        }
    }
}

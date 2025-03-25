using Godot;
using System;

namespace StateMachine.States
{
    public class SeekState : BaseState
    {
        public SeekState(Guard guard)
            : base(guard, "Seek")
        {
        }

        public override void Enter()
        {
            base.Enter();

            _guard.LastKnownPlayerPosition = _guard.Player.Position;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            _guard.SeekLastKnownPlayerPosition(delta);

            if (_guard.Velocity == Vector2.Zero)
            {
                TransitionTo<SearchState>();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;

            // Allow transitions to SearchState
            return targetState is SearchState;
        }
    }
}

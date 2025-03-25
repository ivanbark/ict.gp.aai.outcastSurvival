using Godot;
using System;

namespace StateMachine.States
{
    public class SearchState : BaseState
    {
        private float _searchTime = 0f;
        private Vector2 _searchPosition;
        private bool _searchPositionInitialized = false;

        public SearchState(Guard guard)
            : base(guard, "Search")
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            if (_searchTime >= 3.0f)
            {
                var random = new Random();
                float angle = (float)(random.NextDouble() * 2 * Math.PI);
                float radius = 50f + (float)(random.NextDouble() * 50); // Minimum 50, maximum 100
                _searchPosition = _guard.LastKnownPlayerPosition + new Vector2(
                    radius * (float)Math.Cos(angle),
                    radius * (float)Math.Sin(angle)
                );
                _searchPositionInitialized = true;
                _searchTime = 0f;
            }

            if (_searchPositionInitialized)
            {
                _guard.SearchForPlayer(delta, _searchPosition);
            }
            _searchTime += delta;
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;

            // Allow transitions to PlayerDetectedState or PatrolState
            return targetState is PlayerDetectedState || targetState is PatrolState;
        }
    }
}

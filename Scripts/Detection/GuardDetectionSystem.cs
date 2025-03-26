using Godot;
using System;
using StateMachine.States;
namespace Detection
{
    public class GuardDetectionSystem
    {
        private readonly Guard _guard;
        private readonly float _baseDetectionRange;
        private readonly float _visionAngle;
        private readonly float _visionRange;
        private readonly float _backDetectionMultiplier;
        private readonly float _sideDetectionMultiplier;

        public GuardDetectionSystem(Guard guard, float baseDetectionRange, float visionAngle, float visionRange)
        {
            _guard = guard;
            _baseDetectionRange = baseDetectionRange;
            _visionAngle = visionAngle;
            _visionRange = visionRange;
            _backDetectionMultiplier = 0.3f;
            _sideDetectionMultiplier = 0.6f;
        }

        public bool CanDetectPlayer(Player player)
        {
            if (player == null) return false;

            float distance = _guard.Position.DistanceTo(player.Position);
            float detectionRange = CalculateDetectionRange(player);

            if (distance > detectionRange) return false;

            // Check if player is in vision cone
            Vector2 directionToPlayer = (player.Position - _guard.Position).Normalized();
            float angleToPlayer = Mathf.Abs(_guard.Rotation - directionToPlayer.Angle());

            // Normalize angle to 0-180 range
            if (angleToPlayer > Mathf.Pi)
            {
                angleToPlayer = 2 * Mathf.Pi - angleToPlayer;
            }

            // Calculate detection multiplier based on angle
            float detectionMultiplier = CalculateDetectionMultiplier(angleToPlayer);

            // Final detection check with all factors
            return distance <= detectionRange * detectionMultiplier;
        }

        private float CalculateDetectionRange(Player player)
        {
            // Get player's current movement state noise level
            float playerNoiseLevel = 1.0f; // Default to walking noise level
            if (player.CurrentState is PlayerMovementState movementState)
            {
                playerNoiseLevel = movementState.GetNoiseLevel();
            }

            // Base detection range modified by player's noise level
            return _baseDetectionRange * playerNoiseLevel;
        }

        private float CalculateDetectionMultiplier(float angleToPlayer)
        {
            // Convert vision angle to radians and get half angle
            float halfVisionAngle = _visionAngle * Mathf.Pi / 180f / 2f;

            if (angleToPlayer <= halfVisionAngle)
            {
                // Player is in front of guard
                return 1.0f;
            }
            else if (angleToPlayer <= Mathf.Pi / 2f)
            {
                // Player is to the side of guard
                return _sideDetectionMultiplier;
            }
            else
            {
                // Player is behind guard
                return _backDetectionMultiplier;
            }
        }

        public Vector2 GetLastKnownPlayerPosition()
        {
            return _guard.LastKnownPlayerPosition;
        }

        public void UpdateLastKnownPlayerPosition(Vector2 position)
        {
            _guard.LastKnownPlayerPosition = position;
        }
    }
}

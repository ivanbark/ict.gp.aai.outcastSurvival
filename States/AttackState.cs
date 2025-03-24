using Godot;
using System;

public partial class AttackState : State
{
	private Guard _guard;

	protected override void OnInitialize()
	{
		_guard = fsm.Guard;
	}

	public override void Enter()
	{
		GD.Print("Entering AttackState");
		_guard.animatedSprite.Animation = "attack";
		_guard.animatedSprite.Play();
	}

	public override void Exit()
	{
		GD.Print("Exiting AttackState");
		_guard.animatedSprite.Stop();
	}

	public override void Process(float delta)
	{
		if (_guard == null || _guard.Player == null) return;

		_guard.AttackPlayer(delta);

		if (_guard.Position.DistanceTo(_guard.Player.Position) > _guard.AttackRange)
		{
			TransitionTo("Chase");
		}
	}
}

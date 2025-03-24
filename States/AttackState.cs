using Godot;
using System;
using OutCastSurvival.Entities;

namespace OutCastSurvival.State 
{
public partial class AttackState : State
{
	private Guard _guard;
	
	public override void Ready()
	{
		_guard = fsm.Guard;
	}
	
	public override void Enter()
	{
		_guard.animatedSprite.Animation = "attack";
		_guard.animatedSprite.Play();
	}

	public override void Exit()
	{
		_guard.animatedSprite.Stop();
	}
	
	public override void Process(float delta)
	{
		if (_guard == null || _guard.Player == null) return;
		
		_guard.AttackPlayer(delta);

		if (_guard.Position.DistanceTo(_guard.Player.Position) > 150f)
		{
			fsm.TransitionTo("Chase");
		}
	}
}
}
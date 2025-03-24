using Godot;
using System;

namespace OutCastSurvival.Entities 
{
  public partial class Sheep : MovingEntity
  {
    [Export]
    private float flocking_radius = 2.5f;
    


    public override void _Ready()
    {
      base._Ready();
      
      AddToGroup("Entities");
      AddToGroup("Sheep");
    }
      public override void _Process(double delta)
      {
          base._Process(delta);
          // Player player = World_ref.GetPlayer();
          // if (player == null)
          //   return;
          Vector2 seek_force = SteeringBehaviour.Seek(Position,new(505,550),MaxForce);
          // wander
          Position += seek_force;
          // flocking

      }
  }  
}

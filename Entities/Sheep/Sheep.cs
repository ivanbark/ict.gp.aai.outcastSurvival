using Godot;
using System;

namespace OutCastSurvival.Entities 
{
  public partial class Sheep : MovingEntity
  {
    [Export]
    private float flocking_radius = 25.0f;
    

    [Export]
    private float Separation_force = 50f;
    private Vector2 Separation_force_vector;
    [Export]
    private float Alignment_force = 1f;
    private Vector2 Alignment_force_vector;
    [Export]
    private float Cohesion_force = 10f;
    private Vector2 Cohesion_force_vector;

    public override void _Ready()
    {
      base._Ready();
      MaxForce = 600;
      MaxSpeed = 25;
      
      AddToGroup("Entities");
      AddToGroup("Sheep");
    }
      public override void _Process(double delta)
      {
        // Vector2 seek_force = SteeringBehaviour.Seek(Position,new(1200,550),Separation_force + 0.01f);
        // // wander
        // Velocity += seek_force;
        // flocking

        //get entities in radius from world.
        Sheep[] otherSheep = World_ref.GetOtherSheep(Position, flocking_radius);

        Vector2 separation_vec = new();
        Vector2 cohesion_vec = new();
        int num_sheep = 0;
        foreach(Sheep sheep in otherSheep) 
        {
          num_sheep++;
          // dont compare to the same object
          if (sheep.Equals(this) || sheep.GetHashCode() == GetHashCode())
            continue;
          //seperation
          Vector2 offset = Position - sheep.Position;
          float distance = offset.Length();

          if (distance > 0.01f) // avoid division by zero or jitter at very small distances
          {
              offset = offset.Normalized() / distance; // stronger push when closer
              separation_vec += offset;
          }

          // alignment (not using for now, but implemnt her otherwise)

          // cohesion
          cohesion_vec.X += sheep.Position.X;
          cohesion_vec.Y += sheep.Position.Y;


        }
        // GD.Print(num_sheep);
        if (num_sheep != 0) 
        {
          separation_vec /= num_sheep;
          separation_vec = separation_vec.Normalized();
          Separation_force_vector = separation_vec * Separation_force;

          cohesion_vec /= num_sheep;
          cohesion_vec -= Position;
          cohesion_vec = cohesion_vec.Normalized();
          Cohesion_force_vector = cohesion_vec * Cohesion_force;

        }

        Velocity += Separation_force_vector + Cohesion_force_vector;
        base._Process(delta);

        QueueRedraw();
      }

      public override void _Draw()
      {
        base._Draw();
        if (visualize_debug_info) {
          // orientation and velocity


          //flocking:
          DrawCircle(new(0,0),flocking_radius, Colors.Purple, false, 1);
          //Seperation
          DrawLine(new(),Separation_force_vector, Colors.Yellow, 1);
          //Cohesion
          DrawLine(new(),Cohesion_force_vector, Colors.Blue, 1);
        }
      }

        public override bool Equals(object obj)
        {
           if (obj is Sheep sheep) {
              return (sheep.Position == Position && sheep.Velocity == Velocity);
           }
           return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }  
}

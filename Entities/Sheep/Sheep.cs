using Godot;
using System;
using System.Collections.Generic;

namespace OutCastSurvival.Entities 
{
  public partial class Sheep : MovingEntity
  {
    [Export]
    private float flocking_radius = 25.0f;
    

    [Export]
    private float Separation_force = 50f;
    private Vector2 Separation_force_vector = new(0,0);
    [Export]
    private float Alignment_force = 1f;
    private Vector2 Alignment_force_vector = new(0,0);
    [Export]
    private float Cohesion_force = 10f;
    private Vector2 Cohesion_force_vector = new(0,0);

    // [Export]
    // private int NUM_

    private Rect2 obstacleAvoidanceBox;
    [Export]
    private int widthObstacleAvoidanceBox;
    [Export]
    private int heightObstacleAvoidanceBox;
    
    [Export]
    private float ObstacleAvoidance_force = 10f;
    private Vector2 ObstacleAvoidance_force_vector = new(0,0);
    
    public Obstacle closestObstacle = null;

    public override void _Ready()
    {
      obstacleAvoidanceBox = new(new(0,-heightObstacleAvoidanceBox/2), new(widthObstacleAvoidanceBox,heightObstacleAvoidanceBox));
      
      base._Ready();
      MaxForce = 600;
      MaxSpeed = 25;
      
      AddToGroup("Entities");
      AddToGroup("Sheep");
    }
    public override void _Process(double delta)
    {
      // Vector2 seek_force = SteeringBehaviour.Seek(Position,new(1200,550),Separation_force + 0.01f);
      // Velocity += seek_force;


      // wander

      // flocking
      CalculateFlockingForces();

      // getting obstacle avoidance force

      //get obstacles in box.
      var allObstacles = World_ref.graph_ref.obstacles;
      
      if (World_ref.visualize_debug_info)
        GD.Print("Obstacles:", allObstacles.Count);
      List<Obstacle> obstaclesInBox = [];
      foreach (Obstacle obstacle in allObstacles)
      {
        if (World_ref.visualize_debug_info)
        {
          GD.Print("Obstacle coords: " , obstacle.vertex);
          GD.Print("Box coords: " , obstacleAvoidanceBox);
        }
          

        Rect2 globalSpaceBox = new(Position + obstacleAvoidanceBox.Position, obstacleAvoidanceBox.Size);
        // determine if in box
        if (obstacle.vertex.position.X >= Position.X + globalSpaceBox.Position.X &&
          obstacle.vertex.position.X <= Position.X + globalSpaceBox.Position.X + globalSpaceBox.Size.X &&
          obstacle.vertex.position.Y >= Position.Y + globalSpaceBox.Position.Y &&
          obstacle.vertex.position.Y <= Position.Y + globalSpaceBox.Position.Y + globalSpaceBox.Size.Y)
        {
          Obstacle newObstacle = new(obstacle); // to keep the original obstacle the same
          newObstacle.vertex.position -= (Vector2I)Position; // translate obstacles in box to local space
          obstaclesInBox.Add(newObstacle);
        }

      }
      
      if (World_ref.visualize_debug_info)
        GD.Print("Obstacles in box:", obstaclesInBox.Count);


      float distantTClosestoObstacle = Mathf.Inf;
      foreach (Obstacle obstacle in obstaclesInBox)
      {
        // get closest
        float distance = ((Vector2)obstacle.vertex.position).DistanceTo(Position);
        if ( distance < distantTClosestoObstacle)
        {
          distantTClosestoObstacle = distance;
          closestObstacle = obstacle;
        }
      }
      
      Vector2 avoidance_vec = new();
      // get steering force
      if (closestObstacle != null)
      {
        float multiplier = 1.0f + (distantTClosestoObstacle /(float)widthObstacleAvoidanceBox);

        avoidance_vec.Y = closestObstacle.vertex.position.Y * multiplier;

        float brakeCoefficient = 0.2f;

        avoidance_vec.X = closestObstacle.vertex.position.X * brakeCoefficient; 
      }
      


      ObstacleAvoidance_force_vector = avoidance_vec * ObstacleAvoidance_force;


      // adding all the forces together
      Velocity += Separation_force_vector + Cohesion_force_vector + ObstacleAvoidance_force_vector;
      base._Process(delta);

      QueueRedraw();
    }

    private void CalculateFlockingForces()
    {

      //get entities in radius from world.
      Sheep[] otherSheep = World_ref.GetOtherSheep(Position, flocking_radius);

      Vector2 separation_vec = new();
      Vector2 cohesion_vec = new();
      int num_sheep = 0;
      foreach (Sheep sheep in otherSheep)
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
    }


    public override void _Draw()
    {
      base._Draw();
      if (World_ref.visualize_debug_info) {
        // orientation and velocity


        //flocking:
        // DrawCircle(new(0,0),flocking_radius, Colors.Purple, false, 1);
        //Seperation:
        // DrawLine(new(),Separation_force_vector, Colors.Yellow, 1);
        //Cohesion:
        // DrawLine(new(),Cohesion_force_vector, Colors.Blue, 1);

        //obstacle avoidance:
        DrawRect(obstacleAvoidanceBox,Colors.HotPink, false, 1);
        DrawLine(new(),ObstacleAvoidance_force_vector, Colors.HotPink, 1);
        if (closestObstacle != null)
          DrawCircle(new(0,0),3, Colors.HotPink, true, 1);
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

    protected override void UpdateDebugInfo()
    {
      // Update state label
    if (_stateLabel != null)
    {
      _stateLabel.Text = "";
    }

    // Update health label
    if (_healthLabel != null)
    {
      _healthLabel.Text = "";
    }
    }
  }
}

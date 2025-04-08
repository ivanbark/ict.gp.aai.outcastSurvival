using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OutCastSurvival.Entities
{
  public partial class Sheep : MovingEntity
  {
    [Export]
    private float Cohesion_radius = 45.0f;

    [Export]
    private float Seperation_radius = 15.0f;

    [Export]
    private float SeekForce = 2f;

    [Export]
    private float Separation_force = 50f;
    private Vector2 Separation_force_vector = new(0, 0);
    [Export]
    private float Alignment_force = 1f;
    private Vector2 Alignment_force_vector = new(0, 0);
    [Export]
    private float Cohesion_force = 10f;
    private Vector2 Cohesion_force_vector = new(0, 0);

    // [Export]
    // private int NUM_

    private Rect2 obstacleAvoidanceBox;
    [Export]
    private int widthObstacleAvoidanceBox;
    [Export]
    private int heightObstacleAvoidanceBox;

    [Export]
    private float ObstacleAvoidance_force = 10f;
    private Vector2 ObstacleAvoidance_force_vector = new(0, 0);

    public Obstacle closestObstacle = null;

    public List<Vertex> path = null;

    public override void _Ready()
    {
      base._Ready();
      MaxForce = 600;
      MaxSpeed = 20;
      obstacleAvoidanceBox = new(new(0, -heightObstacleAvoidanceBox / 2), new(widthObstacleAvoidanceBox, heightObstacleAvoidanceBox));

      AddToGroup("Entities");
      AddToGroup("Sheep");
      World_ref.debug_ref.DebugOptionChanged += () => QueueRedraw();
    }
    public override void _Process(double delta)
    {
      if (path == null && World_ref.TargetVertex != null)
      {
        // get graph coords
        int tileSize = World_ref.graph_ref.TileSize;
        Vector2I graphCoords = new((int)Position.X / tileSize, (int)Position.Y / tileSize);

        World_ref.graph_ref.GetVertexForPosition(graphCoords, out Vertex start);
        path = World_ref.graph_ref.A_star(start, World_ref.TargetVertex);
      }
      // else
      // {
      // if (path.Count > 0)
      // {
      //   Position = path[0].position;
      //   path.RemoveAt(0);
      // }
      // }


      // Vector2 seek_force = SteeringBehaviour.Seek(Position, new(1700, 950), SeekForce);
      // Velocity += seek_force;


      // wander

      // flocking
      CalculateFlockingForces();

      // getting obstacle avoidance force

      //get obstacles in box.
      var allObstacles = World_ref.graph_ref.obstacles;
      List<Obstacle> obstaclesInBox = [];
      closestObstacle = null;

      foreach (Obstacle obstacle in allObstacles)
      {
        Rect2 globalSpaceBox = new(Position + obstacleAvoidanceBox.Position, obstacleAvoidanceBox.Size);

        Obstacle globalspaceObstacle = new(obstacle); // to keep the original obstacle the same
        World_ref.graph_ref.TranslateToGlobal(globalspaceObstacle.vertex);

        // determine if in box
        if (globalspaceObstacle.vertex.position.X >= globalSpaceBox.Position.X &&
          globalspaceObstacle.vertex.position.X <= globalSpaceBox.Position.X + globalSpaceBox.Size.X &&
          globalspaceObstacle.vertex.position.Y >= globalSpaceBox.Position.Y &&
          globalspaceObstacle.vertex.position.Y <= globalSpaceBox.Position.Y + globalSpaceBox.Size.Y)
        {
          globalspaceObstacle.vertex.position -= (Vector2I)Position; // translate obstacles in box to local space
          obstaclesInBox.Add(globalspaceObstacle);
        }
      }

      float distantTClosestoObstacle = Mathf.Inf;
      Vector2 avoidance_vec = new();
      Vector2 sumOfLateral = new();
      foreach (Obstacle obstacle in obstaclesInBox)
      {
        // get closest
        float distance = ((Vector2)obstacle.vertex.position).DistanceTo(Position);
        if (distance < distantTClosestoObstacle)
        {
          distantTClosestoObstacle = distance;
          closestObstacle = obstacle;
        }
        sumOfLateral += CalculateLateralVec(obstacle);
      }

      // get steering force
      // if (closestObstacle != null)
      // {
      //   Vector2 lateral_vec = CalculateLateralVec(closestObstacle);

      //   // float multiplier = 1.0f + (distantTClosestoObstacle /widthObstacleAvoidanceBox);

      //   // avoidance_vec.Y = closestObstacle.vertex.position.Y * multiplier;

      //   float brakeCoefficient = 0.2f;

      //   // avoidance_vec.X = closestObstacle.vertex.position.X * brakeCoefficient;
      //   Vector2 braking_vec = Velocity * -brakeCoefficient;
      //   avoidance_vec = braking_vec + lateral_vec;
      // }
      avoidance_vec = sumOfLateral;


      ObstacleAvoidance_force_vector = avoidance_vec * ObstacleAvoidance_force;

      // GD.Print($"Velocity: {Velocity}Seperation: {Separation_force_vector} \nCohesion:{Cohesion_force_vector} \nobstacle avoidance: {ObstacleAvoidance_force_vector}");
      // adding all the forces together
      Velocity += Separation_force_vector + Cohesion_force_vector + ObstacleAvoidance_force_vector;
      base._Process(delta);

      QueueRedraw();
    }

    private Vector2 CalculateLateralVec(Obstacle obstacle)
    {
      // ax+ b
      // a = dx/dy
      // b = 0
      float a = Velocity.X / Velocity.Y;

      // cx+ d = y
      // c = -1/a
      // d = enter obstacle coords
      // d = y - cx
      float c = -1 / a;
      float d = obstacle.vertex.position.Y - c * obstacle.vertex.position.X;

      // intersection

      // ax + b = cx+ d
      // ax += cx+ d
      // x * (a + c) = d
      // x = d / (a +c)
      // y = a * x

      float x = d / (a + c);
      Vector2 intersection = new(x, a * x);

      // vector from obstacle to intersection
      Vector2 lateral_vec = intersection - obstacle.vertex.position;
      return lateral_vec;
    }


    private void CalculateFlockingForces()
    {

      //get entities in radius from world.
      Sheep[] cohesionSheepList = World_ref.GetOtherSheep(Position, Cohesion_radius);
      Sheep[] seprationSheepList = CalculateSeperationSheep(cohesionSheepList, Seperation_radius, out int num_seperation_sheep);

      Vector2 separation_vec = new();
      Vector2 cohesion_vec = new();
      int num_cohesion_sheep = 0;
      foreach (Sheep sheep in seprationSheepList)
      {
        // dont compare to the same object

        //seperation
        Vector2 offset = Position - sheep.Position;
        float distance = offset.Length();

        if (distance > 0.01f) // avoid division by zero or jitter at very small distances
        {
          offset = offset.Normalized() / distance; // stronger push when closer
          separation_vec += offset;
        }

        // alignment (not using for now, but implemnt her otherwise)
      }
      foreach (Sheep sheep in cohesionSheepList)
      {
        num_cohesion_sheep++;
        // cohesion
        cohesion_vec.X += sheep.Position.X;
        cohesion_vec.Y += sheep.Position.Y;
      }

      if (num_seperation_sheep != 0)
      {
        separation_vec /= num_seperation_sheep;
        separation_vec = separation_vec.Normalized();
        Separation_force_vector = separation_vec * Separation_force;
      }

      if (num_cohesion_sheep != 0)
      {
        cohesion_vec /= num_cohesion_sheep;
        cohesion_vec -= Position;
        cohesion_vec = cohesion_vec.Normalized();
        Cohesion_force_vector = cohesion_vec * Cohesion_force;
      }
    }

    private Sheep[] CalculateSeperationSheep(Sheep[] cohesionSheepList, float seperation_radius, out int num_sheep)
    {
      List<Sheep> seperationSheep = [];
      num_sheep = 0;
      foreach (Sheep sheep in cohesionSheepList)
      {
        // if sheep is within the separation_radius, add to the list
        if (sheep.Equals(this) || sheep.GetHashCode() == GetHashCode())
          continue;

        num_sheep++;

        float distance = Position.DistanceTo(sheep.Position);
        if (distance <= seperation_radius)
        {
          seperationSheep.Add(sheep);
        }
      }

      return [.. seperationSheep];
    }

    public override void _Draw()
    {
      base._Draw();
      if (World_ref.debug_ref.ShowDebug)
      {
        //flocking:
        //Seperation:
        if (World_ref.debug_ref.ShowSeperation)
        {
          DrawCircle(new(0, 0), Seperation_radius, Colors.Red, false, 1);
          DrawLine(new(), Separation_force_vector, Colors.Yellow, 1);
        }

        //Cohesion:
        if (World_ref.debug_ref.ShowCohesion)
        {
          DrawCircle(new(0, 0), Cohesion_radius, Colors.Purple, false, 1);
          DrawLine(new(), Cohesion_force_vector, Colors.Blue, 1);
        }
        //obstacle avoidance:
        if (World_ref.debug_ref.ShowObstacleAvoidance)
        {
          DrawRect(obstacleAvoidanceBox, Colors.HotPink, false, 1);
          DrawLine(new(), ObstacleAvoidance_force_vector, Colors.HotPink, 1);
        }
      }
    }

    public override bool Equals(object obj)
    {
      if (obj is Sheep sheep)
      {
        return (sheep.Position == Position && sheep.Velocity == Velocity);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    protected override void Die()
    {
      var meatScene = GD.Load<PackedScene>("res://Items/Meat.tscn");
      var meat = meatScene.Instantiate<Meat>();
      World_ref.AddChild(meat);
      meat.Position = Position;
      QueueFree();
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

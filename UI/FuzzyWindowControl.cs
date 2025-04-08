using Godot;
using OutCastSurvival.Classes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OutCastSurvival
{
  public partial class FuzzyWindowControl : Node2D
  {
    Fuzzyset[] Fuzzysets = new Fuzzyset[4];
    public override void _Ready()
    {
      // var SaveBtn = GetParent().GetNode<Button>("SaveBtn");
      // SaveBtn.Pressed += () => TestSet.Save();
      var parent = GetParent<Window>();
      parent.CloseRequested += () => parent.Visible = false;

      var NoiseLevel = new Fuzzyset("Noise Level");
      var Position = new Fuzzyset("Position");
      var Behaviour = new Fuzzyset("Behaviour");
      if (!NoiseLevel.Load())
      {
        GD.PrintErr("Failed to load Noise Level fuzzy set");
        return;
      }
      if (!Position.Load())
      {
        GD.PrintErr("Failed to load Position fuzzy set");
        return;
      }
      if (!Behaviour.Load())
      {
        GD.PrintErr("Failed to load Behaviour fuzzy set");
        return;
      }
      Fuzzysets[0] = NoiseLevel;
      Fuzzysets[1] = Position;
      Fuzzysets[2] = Behaviour;
    }
    public override void _Process(double delta)
    {
      QueueRedraw();
    }


    public override void _Draw()
    {
      float width = 400;
      float height = 150;
      Vector2 offset = new(40, 0);
      for (int i = 0; i < Fuzzysets.Length; i++)
      {
        int row = i < 2 ? 1 : 2;
        int column = i % 2;
        Vector2 position_offset = offset + new Vector2(0, height * row + row * 64) + new Vector2(column * (width + 40), 0);
        DrawFuzzySet(Fuzzysets[i], position_offset, width, height);
      }

    }
    private void DrawFuzzySet(Fuzzyset fuzzySet, Vector2 position, float width, float height)
    {
      if (fuzzySet == null)
        return;
      // Draw Name
      DrawString(ThemeDB.FallbackFont, position + new Vector2(0, -height - 10), fuzzySet.name, HorizontalAlignment.Center, width, 24, Colors.Black);

      DrawDOMAxis(position, width, height);
      DrawValueAxis(fuzzySet, position, width, height);

      // Draw Functions
      DrawFunctions(fuzzySet, position, width, height);

      // draw function names
      for (int i = 0; i < fuzzySet.functions.Count; i++)
      {
        FuzzyFunction function = fuzzySet.functions.ElementAt(i).Value;
        //Vector2 textPosition = position + new Vector2(function.minValue / fuzzySet.maxValue * width, -function.MembershipValues.Values.ElementAt<float>(0) * height);
        DrawString(ThemeDB.FallbackFont, position + new Vector2(width * ((float)i / fuzzySet.functions.Count), 32), function.name, HorizontalAlignment.Left, -1, 16, function.color);
      }
    }

    private void DrawFunctions(Fuzzyset fuzzySet, Vector2 position, float width, float height)
    {
      foreach (FuzzyFunction function in fuzzySet.functions.Values)
      {
        int length = function.MembershipValues.Count;
        if (length <= 1)
          continue;
        Vector2 lastPoint = GlobalPosition + new Vector2(position.X, position.Y - function.MembershipValues.Values.ElementAt<float>(0) * height);

        for (int i = 1; i < length; i++)
        {
          float value = function.MembershipValues.Keys.ElementAt<float>(i); // X axis
          float dom = function.MembershipValues[value]; // Y axis
          Vector2 currentPoint = new((value - function.minValue) / (function.maxValue - function.minValue) * width + position.X, position.Y - dom * height);
          DrawLine(lastPoint, currentPoint, function.color, 2);
          lastPoint = currentPoint;
        }

      }
    }

    private void DrawDOMAxis(Vector2 position, float width, float height)
    {
      //draw DOM Axis

      DrawLine(position, position + new Vector2(0, -height), Colors.Black, 2);
      Vector2 padding = new(-5, 0);
      DrawStringRightAnchored("1.0", position + padding + new Vector2(0, -height));
      DrawStringRightAnchored("0.5", position + padding + new Vector2(0, -height / 2));
      DrawStringRightAnchored("0", position + padding + new Vector2(0, -10));

      // guide lines
      DrawLine(position + new Vector2(0, -height), position + new Vector2(width, -height), Colors.LightGray, 1);
      DrawLine(position + new Vector2(0, -height / 2), position + new Vector2(width, -height / 2), Colors.LightGray, 1);
    }

    private void DrawValueAxis(Fuzzyset fuzzySet, Vector2 position, float width, float height)
    {
      // Draw value axis
      float maxDistance = fuzzySet.maxValue - fuzzySet.minValue;
      DrawLine(position, position + new Vector2(width, 0), Colors.Black, 2);
      DrawStringTopLeftAnchored(fuzzySet.minValue.ToString(), position + new Vector2(5, 5));
      DrawStringTopLeftAnchored((maxDistance / 4 + fuzzySet.minValue).ToString(), position + new Vector2(width / 4, 5));
      DrawStringTopLeftAnchored((maxDistance / 2 + fuzzySet.minValue).ToString(), position + new Vector2(width / 2, 5));
      DrawStringTopLeftAnchored((3 * maxDistance / 4 + fuzzySet.minValue).ToString(), position + new Vector2(3 * width / 4, 5));
      DrawStringTopLeftAnchored((maxDistance + fuzzySet.minValue).ToString(), position + new Vector2(width, 5));

      // guide lines
      DrawLine(position + new Vector2(width / 4, 0), position + new Vector2(width / 4, -height), Colors.LightGray, 1);
      DrawLine(position + new Vector2(width / 2, 0), position + new Vector2(width / 2, -height), Colors.LightGray, 1);
      DrawLine(position + new Vector2(3 * width / 4, 0), position + new Vector2(3 * width / 4, -height), Colors.LightGray, 1);
      DrawLine(position + new Vector2(width, 0), position + new Vector2(width, -height), Colors.LightGray, 1);
    }

    private void DrawStringRightAnchored(string text, Vector2 position)
    {
      Font font = ThemeDB.FallbackFont;
      var textSize = font.GetStringSize(text);
      DrawString(font, position + new Vector2(-textSize.X, +textSize.Y / 2), text, HorizontalAlignment.Right, -1, 16, Colors.Black);
    }
    private void DrawStringTopLeftAnchored(string text, Vector2 position)
    {
      Font font = ThemeDB.FallbackFont;
      var textSize = font.GetStringSize(text);
      DrawString(font, position + new Vector2(-textSize.X, textSize.Y - 10), text, HorizontalAlignment.Right, -1, 16, Colors.Black);
    }


  }
}


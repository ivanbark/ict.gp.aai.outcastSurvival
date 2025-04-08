using Godot;
using OutCastSurvival.Classes;
using System;

namespace OutCastSurvival
{
  public partial class FuzzyWindowControl : Node2D
  {
    Fuzzyset TestSet;
    public override void _Ready()
    {
      var SaveBtn = GetParent().GetNode<Button>("SaveBtn");
      SaveBtn.Pressed += () => TestSet.Save();

      TestSet = new Fuzzyset("Test Set");
      if (!TestSet.Load())
      {
        var TestFunction = new FuzzyFunction("Test Function", 0, 10);
        TestSet.addFunction(TestFunction);
        TestSet.Save();
      }
    }
    public override void _Process(double delta)
    {
      QueueRedraw();
    }


    public override void _Draw()
    {
      float width = 400;
      float height = 150;
      Vector2 offset = new(40, height + 40);
      // Draw Name
      DrawString(ThemeDB.FallbackFont, offset + new Vector2(0, -height - 10), TestSet.name, HorizontalAlignment.Center, width, 24, Colors.Black);
      //draw DOM Axis
      DrawLine(offset, offset + new Vector2(0, -height), Colors.Black, 2);
      Vector2 padding = new(-5, 0);
      DrawStringRightAnchored("1.0", offset + padding + new Vector2(0, -height));
      DrawStringRightAnchored("0.5", offset + padding + new Vector2(0, -height / 2));
      DrawStringRightAnchored("0", offset + padding + new Vector2(0, -10));

      // guide lines
      DrawLine(offset + new Vector2(0, -height), offset + new Vector2(width, -height), Colors.LightGray, 1);
      DrawLine(offset + new Vector2(0, -height / 2), offset + new Vector2(width, -height / 2), Colors.LightGray, 1);


      // Draw value axis
      DrawLine(offset, offset + new Vector2(width, 0), Colors.Black, 2);
      DrawStringTopLeftAnchored(TestSet.minValue.ToString(), offset + new Vector2(5, 5));
      DrawStringTopLeftAnchored((TestSet.maxValue / 4).ToString(), offset + new Vector2(width / 4, 5));
      DrawStringTopLeftAnchored((TestSet.maxValue / 2).ToString(), offset + new Vector2(width / 2, 5));
      DrawStringTopLeftAnchored((3 * TestSet.maxValue / 4).ToString(), offset + new Vector2(3 * width / 4, 5));
      DrawStringTopLeftAnchored(TestSet.maxValue.ToString(), offset + new Vector2(width, 5));

      // guide lines
      DrawLine(offset + new Vector2(width / 4, 0), offset + new Vector2(width / 4, -height), Colors.LightGray, 1);
      DrawLine(offset + new Vector2(width / 2, 0), offset + new Vector2(width / 2, -height), Colors.LightGray, 1);
      DrawLine(offset + new Vector2(3 * width / 4, 0), offset + new Vector2(3 * width / 4, -height), Colors.LightGray, 1);
      DrawLine(offset + new Vector2(width, 0), offset + new Vector2(width, -height), Colors.LightGray, 1);
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


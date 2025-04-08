using System.Collections.Generic;
using Godot;

namespace OutCastSurvival.Classes
{
  public class FuzzyFunction
  {
    public string name { get; private set; }
    public float minValue { get; private set; }
    public float maxValue { get; private set; }
    public Color color { get; private set; } = Colors.Black;

    public Dictionary<float, float> MembershipValues { get; } = [];


    public FuzzyFunction(string name, float minValue, float maxValue, Color color)
    {
      this.name = name;
      this.minValue = minValue;
      this.maxValue = maxValue;
      this.color = color;
    }

    public void AddMembershipValue(float value, float membershipValue)
    {
      if (!MembershipValues.TryAdd(value, membershipValue))
      {
        MembershipValues[value] = membershipValue;
      }
    }
    public void RemoveMembershipValue(float value)
    {
      MembershipValues.Remove(value);
    }
    public void ClearMembershipValues()
    {
      MembershipValues.Clear();
    }


  }
}
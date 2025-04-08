using System.Collections.Generic;
using Godot;

namespace OutCastSurvival.Classes
{
  public class FuzzyFunction
  {
    public string name { get; private set; }
    public float minValue { get; private set; }
    public float maxValue { get; private set; }

    public Dictionary<float, float> MembershipValues { get; } = [];


    public FuzzyFunction(string name, float minValue, float maxValue)
    {
      this.name = name;
      this.minValue = minValue;
      this.maxValue = maxValue;
    }

    public void AddMembershipValue(float value, float membershipValue)
    {
      if (MembershipValues.ContainsKey(value))
      {
        MembershipValues[value] = membershipValue;
      }
      else
      {
        MembershipValues.Add(value, membershipValue);
      }
    }
    public void RemoveMembershipValue(float value)
    {
      if (MembershipValues.ContainsKey(value))
      {
        MembershipValues.Remove(value);
      }
    }
    public void ClearMembershipValues()
    {
      MembershipValues.Clear();
    }


  }
}
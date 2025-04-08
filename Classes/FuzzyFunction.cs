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

    public float GetMembershipValue(float value)
    {
      GD.Print($"Getting membership value for {value}");
      if (!MembershipValues.ContainsKey(value))
      {
        // sort keys in ascending order
        var sortedKeys = new List<float>(MembershipValues.Keys);
        // sortedKeys.Sort();

        // clamp to min or max if out of bounds
        if (value <= sortedKeys[0])
          return MembershipValues[sortedKeys[0]];
        if (value >= sortedKeys[^1])
          return MembershipValues[sortedKeys[^1]];

        // find the two surrounding keys for interpolation
        for (int i = 0; i < sortedKeys.Count - 1; i++)
        {
          float lowerKey = sortedKeys[i];
          float upperKey = sortedKeys[i + 1];

          if (value >= lowerKey && value <= upperKey)
          {
            float lowerValue = MembershipValues[lowerKey];
            float upperValue = MembershipValues[upperKey];

            float newValue = (value - lowerKey) / (upperKey - lowerKey);
            return Mathf.Lerp(lowerValue, upperValue, newValue);
          }
        }
      }

      // Exact match or fallback
      return MembershipValues.TryGetValue(value, out float membership) ? membership : 0f;
    }

    public float GetDefuzzificationValue()
    {
      float minMax = 100000f;
      float maxmax = 0f;
      foreach (var key in MembershipValues.Keys)
      {
        if (key == 1)
        {
          minMax = Mathf.Min(minMax, MembershipValues[key]);
          maxmax = Mathf.Max(maxmax, MembershipValues[key]);
        }
      }
      return (minMax + maxmax) / 2;
    }
  }
}

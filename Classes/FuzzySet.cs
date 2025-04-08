using System;
using System.Collections.Generic;
using System.Globalization;
using Godot;

namespace OutCastSurvival.Classes
{

  public partial class Fuzzyset
  {

    public string name { get; private set; }
    public float minValue { get; private set; }
    public float maxValue { get; private set; }
    public readonly Dictionary<string, FuzzyFunction> functions = [];

    public void Save()
    {
      using var file = FileAccess.Open($"res://Fuzzy Sets/fuzzySet{name}.txt", FileAccess.ModeFlags.Write);
      if (file != null)
      {
        // Write the data to the file
        file.StoreString(name);
        file.StoreString("\n");
        file.StoreString($"Min value :{minValue.ToString(CultureInfo.InvariantCulture)}");
        file.StoreString("\n");
        file.StoreString($"Max value :{maxValue.ToString(CultureInfo.InvariantCulture)}");
        file.StoreString("\n");
        file.StoreString("Fuzzy Functions\n");
        foreach (FuzzyFunction function in functions.Values)
        {
          file.StoreString($"Fuzzy Function-{function.name}\n");
          foreach (var membershipValue in function.MembershipValues)
          {
            file.StoreString($"{membershipValue.Key.ToString(CultureInfo.InvariantCulture)}:{membershipValue.Value.ToString(CultureInfo.InvariantCulture)}\n");
          }
          file.StoreString("End Fuzzy Function\n");
        }
        file.StoreString("End Fuzzy Functions");
        file.Close();
      }
    }
    public bool Load()
    {
      using var file = FileAccess.Open($"res://Fuzzy Sets/fuzzySet{name}.txt", FileAccess.ModeFlags.Read);
      if (file != null)
      {
        name = file.GetLine();
        minValue = float.Parse(file.GetLine().Split(':')[1], CultureInfo.InvariantCulture);
        maxValue = float.Parse(file.GetLine().Split(':')[1], CultureInfo.InvariantCulture);

        if (file.GetLine() == "Fuzzy Functions")
        {
          string line = file.GetLine();
          string functionName = "";
          FuzzyFunction function = null;

          while (line != "End Fuzzy Functions")
          {
            if (line.StartsWith("Fuzzy Function"))
            {
              functionName = line.Split('-')[1];
              function = new FuzzyFunction(functionName, minValue, maxValue, Color.FromHsv(GD.Randf(), 1, 1));
            }
            else if (line.StartsWith("End Fuzzy Function"))
            {
              if (function != null)
              {
                functions.Add(functionName.Trim(), function);
              }
              function = null;
            }
            else
            {
              string[] parts = line.Split(':');
              if (parts.Length == 2 && function != null)
              {
                float value = float.Parse(parts[0], CultureInfo.InvariantCulture);
                float domValue = float.Parse(parts[1], CultureInfo.InvariantCulture);
                function.AddMembershipValue(value, domValue);
              }
            }
            line = file.GetLine();
          }
        }

        file.Close();

        return true;
      }
      else
      {
        GD.PrintErr($"Failed to load fuzzy set {name}");
        return false;
      }


    }

    public Fuzzyset(string name)
    {
      this.name = name;
      minValue = 0;
      maxValue = 1;
    }

    public void addFunction(FuzzyFunction function)
    {
      minValue = Mathf.Min(minValue, function.minValue);
      maxValue = Mathf.Max(maxValue, function.maxValue);
      functions.Add(function.name, function);
    }
    public void removeFunction(FuzzyFunction function)
    {
      functions.Remove(function.name);
    }
    public void clearFunctions()
    {
      functions.Clear();
    }

  }
}

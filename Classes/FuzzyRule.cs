using System.Collections.Generic;
using Godot;

namespace OutCastSurvival.Classes
{
  public class FuzzyRule
  {
    public string Consequent;
    public string Line;
    public Dictionary<string, string> Antecedents = [];

    public FuzzyRule(string consequent, string line = "")
    {
      Line = line;
      Consequent = consequent;
    }

    public void AddAntecedent(string key, string value)
    {
      Antecedents[key] = value;
    }

  }
}
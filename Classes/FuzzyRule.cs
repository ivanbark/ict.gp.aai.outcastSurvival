using System.Collections.Generic;
using Godot;

namespace OutCastSurvival.Classes
{
  public class FuzzyRule
  {
    public string Consequent;
    public Dictionary<string, string> Antecedents = [];

    public FuzzyRule(string consequent)
    {
      Consequent = consequent;
    }

    public void AddAntecedent(string key, string value)
    {
      Antecedents[key] = value;
    }

  }
}
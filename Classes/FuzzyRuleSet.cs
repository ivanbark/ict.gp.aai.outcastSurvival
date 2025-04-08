using System;
using System.Collections.Generic;
using Godot;
namespace OutCastSurvival.Classes
{
  public partial class FuzzyRuleSet : Node2D
  {

    public List<FuzzyRule> Rules = [];
    public FuzzyRuleSet()
    {
      // Constructor logic here if needed
      Load();
    }

    private void Load()
    {
      using var file = FileAccess.Open($"user://fuzzyRuleSet.txt", FileAccess.ModeFlags.Read);
      if (file != null)
      {
        string line = file.GetLine();
        while (line != "End Fuzzy Rule Set")
        {
          // Process each line of the rule set
          var parts = line.Split(["THEN"], StringSplitOptions.RemoveEmptyEntries);
          var consequent_parts = parts[1];
          var rule = new FuzzyRule(consequent_parts.Split("IS")[1].Trim());

          var antecedent_parts = parts[0];
          var antecedents = antecedent_parts.Split(["AND"], StringSplitOptions.RemoveEmptyEntries);
          foreach (var item in antecedents)
          {
            var Item_parts = item.Split("IS");
            var fuzzySetName = Item_parts[0].Trim();
            var fuzzySetFucntionName = Item_parts[1].Trim();
            rule.AddAntecedent(fuzzySetName, fuzzySetFucntionName);
          }
          GD.Print($"Rule: {line}");
          GD.Print($"Consequent: {rule.Consequent}");
          foreach (var antecedent in rule.Antecedents)
          {
            GD.Print($"Antecedent: {antecedent.Key} IS {antecedent.Value}");
          }
          Rules.Add(rule);

          line = file.GetLine();
        }
        file.Close();
      }
      else
      {
        GD.PrintErr("Failed to load fuzzy rule set file.");
      }
    }
  }
}
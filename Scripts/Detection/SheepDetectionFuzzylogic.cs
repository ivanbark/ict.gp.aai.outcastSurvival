using System.Collections.Generic;
using Godot;
using OutCastSurvival.Classes;
using StateMachine;

namespace OutCastSurvival.Entities.Detection
{
  public class SheepDetectionFuzzyLogic
  {
    // Fuzzy sets
    public Dictionary<string, Fuzzyset> fuzzySets = [];
    public FuzzyRuleSet ruleSet;
    // rules
    // fuzzysets
    // function in fuzzyfunction to get dom
    public SheepDetectionFuzzyLogic()
    {
      fuzzySets["Position"] = new("Position");
      if (!fuzzySets["Position"].Load())
        return;
      fuzzySets["Noise Level"] = new("Noise Level");
      if (!fuzzySets["Noise Level"].Load())
        return;

      fuzzySets["Behaviour"] = new("Behaviour");
      if (!fuzzySets["Behaviour"].Load())
        return;

      ruleSet = new();
      GD.Print(ruleSet.Rules.Count);

    }
    public float calculate(float angle, float noiselevel)
    {
      var rule_doms = new Dictionary<FuzzyRule, float>();
      foreach (var rule in ruleSet.Rules)
      {
        // Get the antecedents for the rule
        var antecedents = rule.Antecedents;
        // Get the consequent for the rule
        var consequent = rule.Consequent;

        float[] doms_atencedents = new float[antecedents.Count];
        int index = 0;
        foreach (var antecedent in antecedents)
        {
          // Get the fuzzy set for the antecedent
          var fuzzySet = fuzzySets[antecedent.Key];
          // Get the fuzzy function for the antecedent
          string value = antecedent.Value.Trim();
          var fuzzyFunction = fuzzySet.functions[value];
          // Get the membership value for the antecedent
          var membershipValue = 0f;
          if (antecedent.Key == "Position")
          {
            membershipValue = fuzzyFunction.GetMembershipValue(angle);
          }
          else if (antecedent.Key == "Noise Level")
          {
            membershipValue = fuzzyFunction.GetMembershipValue(noiselevel);
          }
          doms_atencedents[index++] = membershipValue;
        }
        // Use minimum for AND operation
        var doms_atencedents_result = float.Min(doms_atencedents[0], doms_atencedents[1]);
        rule_doms[rule] = doms_atencedents_result;
      }

      // Aggregate results for each behavior
      var outcome_maxes = new Dictionary<string, float>();
      foreach (var dom in rule_doms)
      {
        if (!outcome_maxes.ContainsKey(dom.Key.Consequent))
        {
          outcome_maxes[dom.Key.Consequent] = dom.Value;
        }
        else
        {
          // Use maximum for OR operation
          outcome_maxes[dom.Key.Consequent] = float.Max(outcome_maxes[dom.Key.Consequent], dom.Value);
        }
      }

      // Defuzzification using weighted average
      float sum = 0f;
      float weightSum = 0f;
      foreach (var outcome in outcome_maxes)
      {
        // Get the defuzzification value based on the behavior
        float defuzzValue = outcome.Key == "Flee" ? -1f : 1f; // -1 for Flee, 1 for Idle
        float weight = outcome.Value;
        sum += defuzzValue * weight;
        weightSum += weight;
      }

      // Return the defuzzified result
      return weightSum == 0 ? 0 : sum / weightSum;
    }

  }
}

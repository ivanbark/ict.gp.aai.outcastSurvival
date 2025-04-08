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
        foreach (var antecedent in antecedents)
        {
          // Get the fuzzy set for the antecedent
          var fuzzySet = fuzzySets[antecedent.Key];
          // Get the fuzzy function for the antecedent
          string value = antecedent.Value;
          var fuzzyFunction = fuzzySet.functions[value];
          // Get the membership value for the antecedent
          var membershipValue = 0f;
          if (value == "Position")
          {
            membershipValue = fuzzyFunction.GetMembershipValue(angle);
          }
          else if (value == "Noise Level")
          {
            membershipValue = fuzzyFunction.GetMembershipValue(noiselevel);
          }
        }
        var doms_atencedents_result = float.Min(doms_atencedents[0], doms_atencedents[1]);
        rule_doms[rule] = doms_atencedents_result;
      }
      var outcome_maxes = new Dictionary<string, float>();
      foreach (var dom in rule_doms)
      {
        if (!outcome_maxes.ContainsKey(dom.Key.Consequent))
        {
          outcome_maxes[dom.Key.Consequent] = dom.Value;
        }
        else
        {
          outcome_maxes[dom.Key.Consequent] = float.Max(outcome_maxes[dom.Key.Consequent], dom.Value);
        }
      }


      // defuzzification
      float sum = 0f;
      float weightSum = 0f;
      foreach (var outcome in outcome_maxes)
      {
        sum += fuzzySets["Behaviour"].functions[outcome.Key].GetDefuzzificationValue() * outcome.Value;
        weightSum += outcome.Value;
      }
      var result = weightSum == 0 ? 0 : sum / weightSum;
      GD.Print($"Result: {result}");

      return result;
    }

  }
}
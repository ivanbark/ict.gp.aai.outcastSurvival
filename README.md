# Outcast Survival – Development Plan
Outcast Survival is a top-down survival game combining elements of stealth, AI-driven
wildlife behavior, and medieval patrol AI. The player, a banished warrior or hunter, must
survive in the wild while avoiding patrols from the kingdom. The game showcases AI
techniques such as steering behaviors, pathfinding, decision-making, and fuzzy logic.

## Tools & Technologies
- **Language:** C# width Godot

## Requirements
- [ ] Game shows a top view of a 2D world
- [ ] The characters in the game show believeble behavior
- [ ] The player can control at least one character
  
- [ ] **Techniques:**
  - [ ] At least 2 complex or composite steering behaviors
  - [ ] Generation of a navigation graph going around obstacles and path planning using A* with suitable heuristic
  - [ ] Making desicions, like controlling the steering behavior and/or path planning
  - [ ] Fuzzy logic needs to be implemented with the following requirements:
    - [ ] at least 2 antecedents and 1 consequent
    - [ ] FLVs are not hard-coded but e.g. read from configuration file, or change in UI/editor
    - [ ] used to control Behaviour (decide which state machine transition, goal-driven, etc, …)

- [ ] **Debug/visualization:**
  - [ ] the user (player) is able to show/hide information for each entity, e.g.: force vectors, graph, path, state, etc.

## Core Gameplay Breakdown



The Wild (Steering & Survival AI)
- The world consists of forests, rivers, and mountains.
- Wild animals (deer, wolves, bears) roam and behave naturally:
- Deer flee when they hear movement.
- Wolves chase prey but avoid humans unless hungry.
- Bears guard their territory but only attack if provoked.
- The player hunts for food using a bow or simple melee combat.
2. Kingdom Patrols (Pathfinding & AI Behavior)
- Guards patrol along paths but react if they hear/see the player.
- If spotted, they chase the player using A* pathfinding.
- If they lose sight, they search the area for a while before returning to patrol.
3. AI Decision-Making (Fuzzy Logic & Goal Selection)
- Guards decide whether to chase, investigate, or return to patrol based on a fuzzy logic
"suspicion level."
- Wild animals choose between fleeing, fighting, or ignoring the player based on their
hunger, threat level, and distance.
Development Plan
Week 1: Core Mechanics & Movement
Choose a game engine (Godot, Unity, or MonoGame).
Implement top-down movement (player & AI).
Basic collision detection with trees, rocks, rivers.
Implement simple patrol AI (guards move along a path).
Week 2: Steering & AI Movement
Implement steering behaviors (animals wander, flee, chase).
AI pathfinding (A* or Dijkstra) for guards & wolves.
Basic player interaction with animals (scare, attack).
Week 3: Decision-Making & AI Behavior
Implement fuzzy logic for AI decision-making.
Guards detect sound & movement (increasing suspicion level).
Animals choose behaviors based on threat & hunger levels.
Week 4: Debugging, Visualizations, & Polish
Add debugging tools (show pathfinding, steering vectors, AI states).
Improve AI reactions (guards searching, animals adapting).
Polish player movement & environment.
Week 5: Final Touches & Submission
Add basic UI & interaction feedback.
Final playtesting & bug fixing.
Write the report (explain AI techniques & design choices).
Tools & Technologies
- **Language:** C# (MonoGame/Godot) or C++ (if using Unreal)
- **Pathfinding:** A* or Dijkstra (for navigation and AI movement)
- **Fuzzy Logic:** Guards’ suspicion level, animals' behavior transitions
- **Steering Behaviors:** Wander, flee, chase for animals and guards
- **Visual Debugging:** Draw patrol paths, AI states, and decision logic
Why This Scope Works
Uses all required AI techniques but keeps the scope manageable.
Two-person team friendly (each person can focus on different AI systems).
Playable & fun in 5 weeks without excessive complexity.

# OutcastSurvival

Top-down 2D survivalgame gebouwd voor het vak Game AI & Algorithms. Doel: verzamel 100 goud, ontwijk bewakers en beheer honger. Focus op AI-technieken: HFSM, pathfinding, fuzzy detection en steering behaviors. 

## Gameplay in één oogopslag
- Win: verzamel 100 goud uit kisten verspreid over de map.
- Verlies: door bewaker gedood of sterven aan honger.
- Kernlussen: sluipen en ontwijken, jagen op schapen voor voedsel, risico vs. beloning.

## Belangrijkste features
- Guard AI met hiërarchische finite state machines (HFSM): Patrol, Alert, PlayerDetected, PlayerLost, Chase, Attack. Substaten en transities met historie. 
- Detectiesysteem op basis van fuzzy logica: afstand, relatieve positie (front/zij/achter), spelersgeluid per bewegingsmodus; 90° zichtkegel; dynamische detectieradius. 
- Pathfinding: A* op tile-graph, waypoint-volgen, dynamische herberekening, box-based obstacle avoidance. (Toegepast op entiteiten zoals schapen.)
- Steering behaviors: Seek, Flee, Arrive; flocking voor schapen (cohesion, separation, alignment) met gewichten en radii.
- Debuginterface: visualisatie van graph en actuele paden, obstakeldetectie, flocking-krachten, vision cones, entity-status; pauze/step keybinds en toggles. 

## Techniek
- Engine: Godot 4.x (.NET/C#).
- Architectuur: nodes en namespaces per verantwoordelijkheid; StateMachine met IState, BaseState en GuardStateMachineNode.

## Installatie en draaien
1. Installeer Godot 4.x .NET build (C# support).
2. Clone deze repo.
3. Open de projectmap in Godot en druk op Run.
4. Voor exports: installeer export templates in Godot en maak een build per platform.

## Status en bekende beperkingen
- Guards gebruiken HFSM maar nog geen A* tijdens achtervolging; geen echte patrolroutes; separation force tussen guards kan beter.
- Navigatiegraph: DFS gaf soms onvolledige dekking; BFS is waarschijnlijk robuuster.

## Roadmap (korte lijst)
- A* pathfinding toevoegen aan guards en patrolroutes configureren.
- Algemeenere state-machinery voor hergebruik in meerdere entities.
- Procedurale maps, dynamische terrein-kosten/zicht, guard-communicatie en afleiding, day-night cycle.

## Debuggen
- Start de game en activeer debugmodus via de in-game keybind.
- Zet overlays aan/uit voor paden, obstakels, vision cones, flocking en entity-info.

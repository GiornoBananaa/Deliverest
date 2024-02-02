using System;

namespace Tutorial
{
    [Serializable]
    public class ScenarioObstacleState: AScenarioState
    {
        public override void Enter()
        {
            Owner.TutorialReferencesContainer.LevelGeneration.OnObstacle += SendExitRequest;
        }

        public override void Update() { }

        public override void Exit()
        {
            Owner.TutorialReferencesContainer.LevelGeneration.OnObstacle -= SendExitRequest;
        }
        
        public override string ToString() => "WaitForObstacle";
    }
}
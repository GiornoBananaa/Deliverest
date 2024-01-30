using Core;
using UnityEngine;

namespace Tutorial
{
    public class TutorialScenario : MonoBehaviour
    {
        [SerializeField] [SerializeReference] 
        private AScenarioState[] _scenarioParts = {
            new ScenarioButtonClickState(),
            new ScenarioSpriteClickState()
        };
        
        private int _currentPartIndex;
        private TutorialStateMachine _tutorialStateMachine;
        private Game _game;

        public void Construct(Game game)
        {
            _game = game;
        }
        
        private void Awake()
        {
            _tutorialStateMachine = new TutorialStateMachine();
        }
        
        private void OnEnable()
        {
            _tutorialStateMachine.StateExitRequest += NextPart;
        }
        
        private void Update()
        {
            _tutorialStateMachine.CurrentState.Update();
        }
        
        public void NextPart()
        {
            if (_currentPartIndex >= _scenarioParts.Length)
                EndTutorial();
            _tutorialStateMachine.ChangeState(_scenarioParts[_currentPartIndex]);
        }
        
        public void EndTutorial()
        {
            _game.StartArcadeGame();
        }
        
        private void OnDisable()
        {
            _tutorialStateMachine.StateExitRequest -= NextPart;
        }
    }
}
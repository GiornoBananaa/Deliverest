using System;
using Core;
using UnityEngine;

namespace Tutorial
{
    public class TutorialScenario : MonoBehaviour
    {
        //TODO put these states in scriptable object and load from bootstrapper
        //---
        [SerializeField] [SerializeReference] 
        private AScenarioState[] _scenarioParts = {
            new ScenarioButtonClickState(),
            new ScenarioSpriteClickState()
        };
        //---
        
        private int _currentPartIndex;
        private TutorialStateMachine _stateMachine;
        //private Game _game;
        
        private void Awake()
        {
            _stateMachine = new TutorialStateMachine();
            _stateMachine.StateExit += NextPart;
        }

        private void Update()
        {
            _stateMachine.CurrentState.Update();
        }

        public void NextPart()
        {
            if (_currentPartIndex >= _scenarioParts.Length)
                EndTutorial();
            _stateMachine.ChangeState(_scenarioParts[_currentPartIndex]);
        }

        public void EndTutorial()
        {
            //_game.StartArcadeGame();
        }
    }
}
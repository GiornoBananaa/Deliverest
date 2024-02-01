using System;
using Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    [Serializable]
    public abstract class AScenarioState : AState
    {
        protected TutorialStateMachine Owner;
        [SerializeField] protected bool _pauseGame;
        [SerializeField] protected Sprite _sprite;

        private Image _dialogueImage;
        private Game _game;

        public void SetOwner(TutorialStateMachine owner) => Owner = owner;
        
        public override void Enter()
        {
            _game = Owner.TutorialReferencesContainer.Game;
            if (_pauseGame)
                _game.Pause(true);
            
            if (_sprite != null)
            {
                _dialogueImage = Owner.TutorialReferencesContainer.DialogueImage;
                Owner.TutorialReferencesContainer.DialogueButton.onClick.AddListener(SendExitRequest);
                _dialogueImage.sprite = _sprite;
                _dialogueImage.gameObject.SetActive(true);
            }
        }

        public override void Exit()
        {
            if (_pauseGame)
                _game.Pause(false);
            if(_dialogueImage!=null)
                _dialogueImage.gameObject.SetActive(false);
        }

        protected void SendExitRequest()
        {
            Exit();
            Owner.StateExitRequest?.Invoke();
        }
    }
}

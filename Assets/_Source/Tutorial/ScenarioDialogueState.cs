using System;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    [Serializable]
    public class ScenarioDialogueState: AScenarioState
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private GameObject[] _activatedObjects;
        private Game _game;
        private Image _dialogueImage;
        
        public override void Enter()
        {
            foreach (var obj in _activatedObjects)
            {
                obj.SetActive(true);
            }
            
            _game = Owner.TutorialReferencesContainer.Game;
            _game.Pause(true);
            
            _dialogueImage = Owner.TutorialReferencesContainer.DialogueImage;
            Owner.TutorialReferencesContainer.DialogueButton.onClick.AddListener(SendExitRequest);
            _dialogueImage.sprite = _sprite;
            _dialogueImage.gameObject.SetActive(true);
        }
        
        public override void Update() { }

        public override void Exit()
        {
            _game.Pause(false);
            Owner.TutorialReferencesContainer.DialogueButton.onClick.RemoveListener(SendExitRequest);
            _dialogueImage.gameObject.SetActive(false);
        }
        
        public override string ToString() => "Dialogue";
    }
}
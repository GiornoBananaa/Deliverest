using System;
using Character;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    [Serializable]
    public class TutorialReferencesContainer
    {
        public Image DialogueImage;
        public Button DialogueButton;
        public CharacterMovement CharacterMovement;
        public Game Game;
        public RockGen LevelGeneration;
    }
}
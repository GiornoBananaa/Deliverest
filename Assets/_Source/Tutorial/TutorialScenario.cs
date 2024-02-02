using Core;
using UnityEditor;
using UnityEngine;

namespace Tutorial
{
    public class TutorialScenario : MonoBehaviour
    {
        [ArrayElementTitleAttribute]
        [SerializeField] [SerializeReference] 
        private AScenarioState[] _scenarioParts = {
            new ScenarioDialogueState(),
            new ScenarioRockHookState(),
            new ScenarioDialogueState(),
            new ScenarioRockHookState(),
            new ScenarioDialogueState(),
            new ScenarioRockHookState(),
            new ScenarioRockHookState(),
            new ScenarioDialogueState(),
            new ScenarioHeightState(),
            new ScenarioDialogueState(),
        };
        
        [SerializeField] private TutorialReferencesContainer _tutorialReferencesContainer;
        
        private int _currentPartIndex;
        private TutorialStateMachine _tutorialStateMachine;
        private Game _game;
        
        public void Construct(Game game)
        {
            _game = game;
            _tutorialReferencesContainer.Game = game;
            _tutorialStateMachine = new TutorialStateMachine();
            _tutorialStateMachine.SetTutorialReferences(_tutorialReferencesContainer);
            _currentPartIndex = -1;
            _tutorialStateMachine.StateExitRequest += NextPart;
            NextPart();
        }
        
        private void Update()
        {
            _tutorialStateMachine.CurrentState.Update();
        }
        
        public void NextPart()
        {
            _currentPartIndex++;
            if (_currentPartIndex >= _scenarioParts.Length)
                EndTutorial();
            else
                _tutorialStateMachine.ChangeState(_scenarioParts[_currentPartIndex]);
        }
        
        public void EndTutorial()
        {
            _game.StartArcadeGame();
        }
        
        private void OnDestroy()
        {
            _tutorialStateMachine.StateExitRequest -= NextPart;
        }
    }
    
    public class ArrayElementTitleAttribute : PropertyAttribute
    {
    }
    
    [CustomPropertyDrawer(typeof(ArrayElementTitleAttribute))]
    public class ArrayElementTitleAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
       
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty nameProp = property.serializedObject.FindProperty(property.propertyPath);
            if (nameProp != null && nameProp.managedReferenceValue != null)
                label = new GUIContent(label) { text = GetTitle(nameProp) };
           
            EditorGUI.PropertyField(position, property, label, true);
        }
       
        private string GetTitle(SerializedProperty prop)
        {
            return prop.managedReferenceValue.ToString();
        }
    }
}
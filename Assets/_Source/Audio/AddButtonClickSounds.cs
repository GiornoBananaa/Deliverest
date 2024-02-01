using Audio;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    [ExecuteInEditMode]
    public class AddButtonClickSounds
    {
        private const string _audioPlayerPropertyName = "";
        
        [MenuItem("Tools/Audio/AddButtonSoundInScene")]
        static void AddSoundForButton()
        {
            AudioPlayer audioPlayer = Object.FindObjectOfType<AudioPlayer>(true);
            
            DeleteSoundForButton();
            Button[] go;
            
            go = Object.FindObjectsOfType<Button>(true);
            
            foreach (Button child in go)
            {
                ButtonSound buttonSound = child.gameObject.AddComponent<ButtonSound>();

                SerializedObject obj = new SerializedObject(buttonSound);

                obj.FindProperty(_audioPlayerPropertyName).objectReferenceValue = audioPlayer;
                obj.ApplyModifiedProperties();
                Debug.Log($"{child.name} button added sound success!");
            }
        }

        [MenuItem("Tools/Audio/ClearAllButtonSoundInScene")]
        static void DeleteSoundForButton()
        {
            Button[] go;
            
            go = Object.FindObjectsOfType<Button>(true);
        
            foreach (Button child in go)
            {
                Object.DestroyImmediate(child.GetComponent<ButtonSound>());
                Debug.Log($"{child.name} button removes sound successfully!");
            }
        }
    }
}
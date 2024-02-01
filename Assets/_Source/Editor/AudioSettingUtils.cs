using Audio;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    [ExecuteInEditMode]
    public class AudioSettingUtils
    {
        private const string _audioPlayerPropertyName = "AudioPlayer";
        private const string _buttonPropertyName = "_button";
        
        [MenuItem("Tools/Audio/SetAudioPlayerProperty")]
        static void SetAudioPlayerProperty()
        {
            AudioPlayer audioPlayer = Object.FindObjectOfType<AudioPlayer>(true);
            
            DeleteSoundForButtons();
            SoundPlayer[] soundPlayers;
            
            soundPlayers = Object.FindObjectsOfType<SoundPlayer>(true);
            
            foreach (SoundPlayer soundPlayer in soundPlayers)
            {
                SerializedObject obj = new SerializedObject(soundPlayer);

                obj.FindProperty(_audioPlayerPropertyName).objectReferenceValue = audioPlayer;
                obj.ApplyModifiedProperties();
                Debug.Log($"{soundPlayer.name} AudioPlayer property set!");
            }
        }
        
        [MenuItem("Tools/Audio/AddButtonSoundInScene")]
        static void AddSoundForButtons()
        {
            AudioPlayer audioPlayer = Object.FindObjectOfType<AudioPlayer>(true);
            
            DeleteSoundForButtons();
            Button[] buttons;
            
            buttons = Object.FindObjectsOfType<Button>(true);
            
            foreach (Button child in buttons)
            {
                ButtonSound buttonSound = child.gameObject.AddComponent<ButtonSound>();

                SerializedObject obj = new SerializedObject(buttonSound);

                obj.FindProperty(_audioPlayerPropertyName).objectReferenceValue = audioPlayer;
                obj.FindProperty(_buttonPropertyName).objectReferenceValue = child;
                obj.ApplyModifiedProperties();
                Debug.Log($"{child.name} button added sound success!");
            }
        }

        [MenuItem("Tools/Audio/ClearAllButtonSoundInScene")]
        static void DeleteSoundForButtons()
        {
            Button[] buttons;
            
            buttons = Object.FindObjectsOfType<Button>(true);
        
            foreach (Button child in buttons)
            {
                Object.DestroyImmediate(child.GetComponent<ButtonSound>());
                Debug.Log($"{child.name} button removes sound successfully!");
            }
        }
    }
}
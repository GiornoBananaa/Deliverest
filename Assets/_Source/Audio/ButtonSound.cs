using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    [RequireComponent(typeof(Button))]
    public class ButtonSound : SoundPlayer
    {
        [SerializeField] private Button _button;
        
        private void PlayClickSound()
        {
            AudioPlayer.Play("button_click");
        }
        
        private void Awake()
        {
            if (_button != null)
            {
                _button.onClick.AddListener(PlayClickSound);
            }
        }
    }
}
using UnityEngine;

namespace LevelGeneration
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GripHighlight : MonoBehaviour
    {
        [SerializeField] private Color _highlightedColor;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private Color _defaultColor;
        
        private void Awake()
        {
            _defaultColor = _spriteRenderer.color;
        }
        
        private void OnMouseEnter()
        {
            _spriteRenderer.color = _highlightedColor;
        }
        
        private void OnMouseExit()
        {
            _spriteRenderer.color = _defaultColor;
        }
    }
}

using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HeightArcadeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _heightText;

        private void FixedUpdate()
        {
            _heightText.text = $"Height: {(int)GameManager.instance.height}m";
        }
    }
}

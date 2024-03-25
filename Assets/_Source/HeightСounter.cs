using System;
using UnityEngine;

public class HeightСounter: MonoBehaviour
{
    [SerializeField] private Transform _target;

    public Action<float> OnHeightChange;
    
    public float Height { get; private set; }
    
    private void Update()
    {
        Height = _target.position.y;
        OnHeightChange.Invoke(Height);
    }
}
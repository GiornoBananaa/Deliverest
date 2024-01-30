using System;
using UnityEngine;

public class Timer
{
    private float _elapsedTime;
    private float _maxTime;
    private bool _isStopped;
    
    public Action OnTimerEnd;
    public Action<float> OnTimeChanged;
    
    public void Update()
    {
        if(_isStopped) return;
        AddTime();
    }
    
    public void Stop() => _isStopped = true;
    
    public void Continue() => _isStopped = false;

    private void AddTime()
    {
        _elapsedTime -= Time.deltaTime;
        if (_elapsedTime > _maxTime)
        {
            _elapsedTime = _maxTime;
            OnTimerEnd?.Invoke();
            Stop();
        }
        
        OnTimeChanged?.Invoke(_elapsedTime);
    }
}
using System;
using UnityEngine;

namespace Core
{
    public class UpdateTimer
    {
        private bool _isStopped;
        
        public Action OnTimerEnd;
        public Action<float> OnTimeChanged;
        
        public float ElapsedTime { get; private set; }
        public float MaxTime { get; private set; }
        
        public bool IsTimerEnd => ElapsedTime >= MaxTime;

        public UpdateTimer(float maxTime)
        {
            MaxTime = maxTime;
        }

        public void SetMaxTime(float maxTime)
        {
            ElapsedTime = maxTime*(ElapsedTime / MaxTime);
            MaxTime = maxTime;
        }
        
        public void Update()
        {
            if(_isStopped) return;
            AddTime();
        }
    
        public void Stop() => _isStopped = true;
    
        public void Continue() => _isStopped = false;
        
        public void Restart()
        {
            ElapsedTime = 0;
            _isStopped = false;
        }
        
        private void AddTime()
        {
            ElapsedTime += Time.deltaTime;
            if (ElapsedTime > MaxTime)
            {
                ElapsedTime = MaxTime;
                OnTimerEnd?.Invoke();
                Stop();
            }
        
            OnTimeChanged?.Invoke(ElapsedTime);
        }
    }
}
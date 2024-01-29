using System;
using Core;
using UnityEngine;

namespace Tutorial
{
    [Serializable]
    public abstract class AScenarioState: AState
    {
        [SerializeField] protected bool _pauseGame;
    }
}

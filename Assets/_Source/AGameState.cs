using Core;
using UnityEngine.SceneManagement;

/*
    NEW GAME STATE SYSTEM IN PROGRESS
*/


public abstract class AGameState: AState
{
    protected IStateMachine<AState> StateMachine;
    
    public void SetOwner(IStateMachine<AState> stateMachine)
    {
        StateMachine = stateMachine;
    }

    public override void Enter()
    {
        
    }
    
    public override void Exit()
    {
        SaveValues();
    }

    public abstract void Restart();
    
    public abstract void Pause();
    
    public abstract void UnPause();
    
    protected abstract void SaveValues();
}

public class ArcadeGameState: AGameState
{
    public HeightСounter _heightСounter;
    
    public ArcadeGameState(HeightСounter heightСounter)
    {
        _heightСounter = heightСounter;
    }
    
    public void LoseGame()
    {
        
    }
    
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public override void Enter()
    {
        
    }

    public override void Update() { }

    public override void Restart()
    {
        
    }

    public override void Pause()
    {
        
    }

    public override void UnPause()
    {
        
    }

    protected override void SaveValues()
    {
        
    }
}

using System;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public static AudioPlayer AudoPlayer = null;
    public Level[] levels;
    [SerializeField] private Level tutorial;
    [SerializeField] private ArcadePanelController losePanel;
    public Level currentLevel
    {
        get
        {
            if (SceneManager.GetActiveScene().buildIndex == 9) return tutorial;
            return levels[currentLevelID];
        }
    }

    private int currentLevelID;
    [HideInInspector] public float best_height;
    public float height
    {
        get => _height;
        set
        {
            _height = value;
            if (_height >= currentLevel.target_height)
                OnTopReached?.Invoke();
        }
    }
    [HideInInspector] public bool isPaused;
    private bool _isSnowStorm;
    public bool isSnowStorm
    {
        get => _isSnowStorm;
        set
        {
            if(!_isSnowStorm && value)
                OnSnowStormStart?.Invoke();
            if(_isSnowStorm && !value)
                OnSnowStormEnd?.Invoke();
            _isSnowStorm = value;
        }
    }
    
    private float _height;

    public Action OnSnowStormStart;
    public Action OnSnowStormEnd;
    public Action OnTopReached;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        LoadValues();
        if (AudoPlayer is null)
        {
            AudoPlayer = GameObject.Find("AudioPlayer").GetComponent<AudioPlayer>();
            DontDestroyOnLoad(AudoPlayer.gameObject);
        }
    }

    public void StartNewGame()
    {
        isPaused = false;
        isSnowStorm = false;
        ResetProgress();
        SceneManager.LoadScene(1);
    }
    public void NextLevel()
    {
        
        if(++currentLevelID >= levels.Length)
        {
            currentLevelID = 0;
            OpenMainMenu();
        }else
        {
            SceneManager.LoadScene(1);
        }

    }

    public void LoseGame(bool loseTime)
    {
        if (height > best_height)
            best_height = height;
        if (loseTime)
            SceneManager.LoadScene(4);
        else
            losePanel.ActivatePanel();
            //SceneManager.LoadScene(2);
    }
    public void WinGame()
    {
        if (height > best_height)
            best_height = height;

        SceneManager.LoadScene(currentLevel.levelWinSceneID);
    }
    public void RestartLevel()
    {
        isPaused = false;
        isSnowStorm = false;
        if (height > best_height)
            best_height = height;

        if(currentLevelID >= 1)
        {
            height = levels[currentLevelID - 1].target_height;
        }
        else
        {
            height = 0;
        }

        SceneManager.LoadScene(1);
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetProgress()
    {
        height = 0;
        currentLevelID = 0;
    }

    private void SaveValues()
    {
        PlayerPrefs.SetFloat("bestHeight", best_height);
    }
    private void LoadValues()
    {
        best_height = PlayerPrefs.GetFloat("bestHeight");
    }
    private void OnApplicationQuit()
    {
        SaveValues();
    }
}

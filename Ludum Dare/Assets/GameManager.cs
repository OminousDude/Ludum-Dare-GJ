using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text floorLevel;
    public static GameManager Instance;
    public GameState currentState;
    public static event Action<GameState> onStateChange;
    private int currentLevel;
    public int numberEnemies;
    public int maxEnemies;
    public int numberDeadEnemies;
    public int capacityEnemies;
    private bool levelChanged;

    void Awake(){
        if (Instance == null) // If there is no instance already
        {
            DontDestroyOnLoad(gameObject); // Keep the GameObject, this component is attached to, across different scenes
            Instance = this;
        }
        else if (Instance != this) // If there is already an instance and it's not `this` instance
        {
            Destroy(gameObject); // Destroy the GameObject, this component is attached to
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance.levelChanged = false;
        Instance.maxEnemies = 6;
        Instance.currentLevel = 1;
        Instance.capacityEnemies = Instance.maxEnemies/2;
        Instance.numberEnemies = 0;
        Instance.numberDeadEnemies = 0;
        Instance.UpdateGameState(GameState.StartMenu);
        Instance.floorLevel.text = Instance.currentLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Instance.numberDeadEnemies == Instance.maxEnemies && !Instance.levelChanged)
        {
            Instance.levelChanged = true;
            Instance.UpdateGameState(GameState.LevelTransition);
        }
        if(Instance.numberEnemies >= Instance.capacityEnemies * 0.7)
        {
            Instance.UpdateGameState(GameState.Warning);
        }
        if (Instance.currentState == GameState.Warning && Instance.numberEnemies < Instance.capacityEnemies * 0.7)
        {
            Instance.UpdateGameState(GameState.Alive);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Instance.UpdateGameState(GameState.PauseMenu);
        }
        if(Instance.numberEnemies == Instance.capacityEnemies)
        {
            Instance.UpdateGameState(GameState.GameOver);
        }
    }

    public void UpdateGameState(GameState newState)
    {
        Instance.currentState = newState;

        switch (newState)
        {
            case GameState.StartMenu:

                break; 
            case GameState.PauseMenu:
                break;
            case GameState.LevelTransition:
                Instance.currentLevel++;
                Instance.floorLevel.text = Instance.currentLevel.ToString();
                Instance.maxEnemies = 6 * Instance.currentLevel;
                Instance.numberEnemies = Instance.maxEnemies;
                Instance.UpdateGameState(GameState.Alive);
                break; 
            case GameState.Warning:
                break; 
            case GameState.Alive:
                Instance.levelChanged = false;
                break; 
            case GameState.GameOver:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }

        onStateChange?.Invoke(newState);
    }

    public enum GameState
    {
        StartMenu,
        PauseMenu,
        LevelTransition,
        Warning,
        Alive,
        GameOver,
    }
}

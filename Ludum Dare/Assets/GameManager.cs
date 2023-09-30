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
    private int numberEnemies;
    private int maxEnemies;
    private int numberDeadEnemies;
    private int capacityEnemies;
    private bool levelChanged;

    void Awake(){
        Instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        levelChanged = false;
        maxEnemies = 6;
        currentLevel = 1;
        capacityEnemies = maxEnemies/2;
        numberEnemies = 0;
        numberDeadEnemies = 0;
        UpdateGameState(GameState.StartMenu);
        floorLevel.text = currentLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (numberDeadEnemies == maxEnemies && !levelChanged)
        {
            levelChanged = true;
            UpdateGameState(GameState.LevelTransition);
        }
        if(numberEnemies >= capacityEnemies * 0.7)
        {
            UpdateGameState(GameState.Warning);
        }
        if (currentState == GameState.Warning && numberEnemies < capacityEnemies * 0.7)
        {
            UpdateGameState(GameState.Alive);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UpdateGameState(GameState.PauseMenu);
        }
        if(numberEnemies == capacityEnemies)
        {
            UpdateGameState(GameState.GameOver);
        }
    }

    public void UpdateGameState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.StartMenu:

                break; 
            case GameState.PauseMenu:
                break;
            case GameState.LevelTransition:
                currentLevel++;
                floorLevel.text = currentLevel.ToString();
                maxEnemies = 6 * currentLevel;
                numberEnemies = maxEnemies;
                UpdateGameState(GameState.Alive);
                break; 
            case GameState.Warning:
                break; 
            case GameState.Alive:
                levelChanged = false;
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

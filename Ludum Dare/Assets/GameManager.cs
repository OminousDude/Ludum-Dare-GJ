using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI floorLevel;
    public static GameManager Instance;
    public GameState currentState;
    public static event Action<GameState> onStateChange;
    public int currentLevel;
    public int numberEnemies;
    public int maxEnemies;
    private int numberDeadEnemies;
    public int capacityEnemies;
    private bool levelChanged;
    public GameObject pauseObject;
    private bool pauseIsActive;

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
        Instance.pauseIsActive = false;
        Instance.levelChanged = false;
        Instance.maxEnemies = 6;
        Instance.currentLevel = 1;
        Instance.capacityEnemies = Instance.maxEnemies/2;
        Instance.numberEnemies = 0;
        Instance.numberDeadEnemies = 0;
        Instance.UpdateGameState(GameState.StartMenu);
        Instance.floorLevel.text = Instance.currentLevel.ToString();
        Instance.pauseObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Instance.numberDeadEnemies == Instance.maxEnemies && !Instance.levelChanged)
        {
            levelChanged = true;
            UpdateGameState(GameState.LevelTransition);
        }
        if(Instance.numberEnemies >= Instance.capacityEnemies * 0.7)
        {
            UpdateGameState(GameState.Warning);
        }
        if (Instance.currentState == GameState.Warning && Instance.numberEnemies < Instance.capacityEnemies * 0.7)
        {
            UpdateGameState(GameState.Alive);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UpdateGameState(GameState.PauseMenu);
        }
        if(Instance.numberEnemies == Instance.capacityEnemies)
        {
            UpdateGameState(GameState.GameOver);
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
                pauseIsActive = !pauseIsActive;
                pauseObject.SetActive(pauseIsActive);
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
    public void resumeIsPressed()
    {
        Instance.pauseIsActive = !pauseIsActive;
        Instance.pauseObject.SetActive(pauseIsActive);
        Instance.UpdateGameState(GameState.Alive);
    }   
    public void restartIsPressed()
    {
        Instance.pauseIsActive = false;
        Instance.levelChanged = false;
        Instance.maxEnemies = 6;
        Instance.currentLevel = 1;
        Instance.capacityEnemies = Instance.maxEnemies / 2;
        Instance.numberEnemies = 0;
        Instance.numberDeadEnemies = 0;
        Instance.floorLevel.text = Instance.currentLevel.ToString();
        Instance.pauseObject.SetActive(false);
        Instance.UpdateGameState(GameState.Alive);
    }   
    public void homeIsPressed()
    {
       //Go back to main menu screen
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

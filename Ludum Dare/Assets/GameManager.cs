using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI floorLevel;
    public static GameManager Instance;
    public GameObject instrMenu;
    public GameObject gameOvrMenu;
    public TextMeshProUGUI gameOvrScore;
    public GameState currentState;
    public static event Action<GameState> onStateChange;
    public int currentLevel;
    public int numberEnemies;
    public int maxEnemies;
    public int numberDeadEnemies;
    public int capacityEnemies;
    private bool levelChanged;
    public GameObject pauseObject;
    private bool pauseIsActive;
    private bool isActive;
    [SerializeField] AudioSource alarmSound;

    void Awake()
    {
        if (Instance == null) // If there is no instance already
        {
            // DontDestroyOnLoad(gameObject); // Keep the GameObject, this component is attached to, across different scenes
            Instance = this;
        }
        else if (Instance != this) // If there is already an instance and it's not `this` instance
        {
            // Destroy(gameObject); // Destroy the GameObject, this component is attached to
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ONI CHAN");
        Instance.pauseIsActive = false;
        Instance.levelChanged = false;
        Instance.maxEnemies = 6;
        Instance.currentLevel = 1;
        Instance.capacityEnemies = Instance.maxEnemies - (Instance.maxEnemies / 3);
        Instance.numberEnemies = 0;
        Instance.numberDeadEnemies = 0;
        Instance.UpdateGameState(GameState.InstructionsMenu);
        Instance.floorLevel.text = Instance.currentLevel.ToString();
        Instance.pauseObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {;
        bool stateInstr = Instance.currentState == GameState.InstructionsMenu;
        bool statePause = Instance.currentState == GameState.PauseMenu;

        if (Instance.numberDeadEnemies == Instance.maxEnemies && !Instance.levelChanged && !statePause)
        {
            Instance.levelChanged = true;
            Instance.UpdateGameState(GameState.LevelTransition);
        }
        if (Instance.numberEnemies >= Instance.capacityEnemies * 0.7 && !statePause && Instance.currentState != GameState.Warning)
        {
            alarmSound.loop = true;
            alarmSound.Play();
            Instance.UpdateGameState(GameState.Warning);
        }
        if (Instance.currentState == GameState.Warning && Instance.numberEnemies < Instance.capacityEnemies * 0.7 && !statePause)
        {
            alarmSound.loop = false;
            Instance.UpdateGameState(GameState.Alive);
        }
        if (Instance.numberEnemies == Instance.capacityEnemies && !statePause)
        {
            alarmSound.loop = false;
            alarmSound.Stop();
            Instance.UpdateGameState(GameState.GameOver);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !stateInstr)
        {
            Instance.UpdateGameState(GameState.PauseMenu);
            Instance.isActive = false;
        }
        if (Input.GetKeyDown(KeyCode.P) && stateInstr)
        {
            stateInstr = false;
            Instance.UpdateGameState(GameState.Alive);
        }
        if (Instance.isActive)
        {
            Instance.UpdateGameState(GameState.Alive);
        }
       
    }

    public void UpdateGameState(GameState newState)
    {
        Instance.currentState = newState;
        onStateChange?.Invoke(newState);
        // Debug.Log(currentState);
        switch (newState)
        {
            case GameState.StartMenu:
                SceneManager.LoadScene("MainMenu");
                break;
            case GameState.PauseMenu:
                Instance.pauseIsActive = !pauseIsActive;
                Instance.pauseObject.SetActive(pauseIsActive);

                Time.timeScale = pauseIsActive?0:1;
                if (!pauseIsActive)
                {
                    Instance.isActive = true;
                    Instance.currentState = GameState.Alive;
                    onStateChange?.Invoke(Instance.currentState);
                }
                else
                    Instance.isActive = false;
                break;
            case GameState.LevelTransition:
                Instance.currentLevel++;
                Instance.floorLevel.text = Instance.currentLevel.ToString();
                Instance.maxEnemies = 6 * Instance.currentLevel;
                Instance.capacityEnemies = Instance.maxEnemies - (Instance.maxEnemies / 3);
                Instance.numberEnemies = 0;
                Instance.numberDeadEnemies = 0;
                Instance.UpdateGameState(GameState.Alive);
                break; 
            case GameState.Warning:
                Time.timeScale = 1;
                break; 
            case GameState.Alive:
                Time.timeScale = 1;
                Instance.instrMenu.SetActive(false);
                Instance.levelChanged = false;
                break;
            case GameState.InstructionsMenu:
                Time.timeScale = 0;
                Instance.instrMenu.SetActive(true);
                break;
            case GameState.GameOver:
                Instance.gameOvrScore.text = "SCORE : LEVEL " + Instance.currentLevel;
                Time.timeScale = 0;
                Instance.gameOvrMenu.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }
       
    }

    public void resumeIsPressed()
    {
        Time.timeScale = 1;
        Instance.pauseIsActive = !pauseIsActive;
        Instance.pauseObject.SetActive(pauseIsActive);
        Instance.UpdateGameState(GameState.Alive);
    }   
    public void restartIsPressed()
    {
        Time.timeScale = 1;
        Instance.pauseIsActive = false;
        Instance.levelChanged = false;
        Instance.maxEnemies = 6;
        Instance.currentLevel = 1;
        Instance.capacityEnemies = Instance.maxEnemies - (Instance.maxEnemies / 3);
        Instance.numberEnemies = 0;
        Instance.numberDeadEnemies = 0;
        Instance.floorLevel.text = Instance.currentLevel.ToString();
        Instance.pauseObject.SetActive(false);
        Instance.gameOvrMenu.SetActive(false);
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject obj in allObjects)
        {
            Destroy(obj);
        }
        Instance.UpdateGameState(GameState.Alive);
    }   
    //public void homeIsPressed()
    //{
    //    Time.timeScale = 1;
    //    Instance.UpdateGameState(GameState.StartMenu);
    //}

    public enum GameState
    {
        StartMenu,
        InstructionsMenu,
        PauseMenu,
        LevelTransition,
        Warning,
        Alive,
        GameOver,
    }
}

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
    [SerializeField] TextMeshProUGUI enemiesLeft;
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
    public GameObject pauseObject;
    private bool pauseIsActive;
    private bool isActive;
    [SerializeField] AudioSource alarmSound;
    public Animator animator2;
    private float time = 0;
    private int count = 0;
    private float timer = 0f;
    private float waitTime = 10f;

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
        Instance.pauseIsActive = false;
        Instance.maxEnemies = 6;
        Instance.currentLevel = 1;
        Instance.capacityEnemies = Instance.maxEnemies - (Instance.maxEnemies / 3);
        Instance.numberEnemies = 0;
        Instance.numberDeadEnemies = maxEnemies;
        Instance.UpdateGameState(GameState.InstructionsMenu);
        Instance.floorLevel.text = Instance.currentLevel.ToString();
        Instance.enemiesLeft.text = "Enemies Left: " + Instance.numberDeadEnemies.ToString();
        Instance.pauseObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Instance.enemiesLeft.text = "Enemies Left: " + Instance.numberDeadEnemies.ToString();
        bool stateInstr = currentState == GameState.InstructionsMenu;
        bool statePause = currentState == GameState.PauseMenu;

        if (Instance.numberDeadEnemies == 0 && !statePause)
        {
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
       
    }

    public void UpdateGameState(GameState newState)
    {
        Instance.currentState = newState;
        onStateChange?.Invoke(newState);
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
                    Instance.UpdateGameState(GameState.Alive);
                }
                break;
            case GameState.LevelTransition:
                Instance.currentLevel++;
                animator2.SetBool("openDoors", false);
                Instance.floorLevel.text = Instance.currentLevel.ToString();
                Instance.maxEnemies = 6 * Instance.currentLevel;
                Instance.capacityEnemies = Instance.maxEnemies - (Instance.maxEnemies / 3);
                Instance.numberEnemies = 0;
                Invoke("Wait", 5);
                break; 
            case GameState.Warning:
                Time.timeScale = 1;
                break; 
            case GameState.Alive:
                Time.timeScale = 1;
                Instance.instrMenu.SetActive(false);
                //if (!Instance.isActive)
                //{
                animator2.SetBool("sleeping", false);
                animator2.SetBool("openDoors", true);
                
                //animator2.SetBool("openDoors", false);
                //}
                isActive = false;
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

    public void Wait()
    {
        Debug.Log("Waited");
        Instance.UpdateGameState(GameState.Alive);
    }
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

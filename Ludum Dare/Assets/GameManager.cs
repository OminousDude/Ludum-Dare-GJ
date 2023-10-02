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
    public GameObject player;
    public GameObject gameOvrMenu;
    public GameObject buildings;
    public GameObject greenBg;
    public GameObject startMenu;
    public GameObject musicScene;
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
    [SerializeField] AudioSource alarmSound;
    [SerializeField] AudioSource menuMusic;
    [SerializeField] AudioSource gameMusic;
    public Animator animator2;

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
        Instance.UpdateGameState(GameState.StartMenu);
        Instance.startMenu.SetActive(true);
        Instance.musicScene.SetActive(false);
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
        bool stateMenu = currentState == GameState.StartMenu;
        if (stateMenu)
        {   
            if (!menuMusic.isPlaying)
                menuMusic.Play();
            gameMusic.Stop();
            menuMusic.loop = true;
        }
        else
        {
            menuMusic.Stop();
            gameMusic.loop = true;
            if(!gameMusic.isPlaying)
                gameMusic.Play();
            
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
            }
            if (Input.GetKeyDown(KeyCode.P) && stateInstr)
            {
                stateInstr = false;
                Instance.UpdateGameState(GameState.Alive);
            }
        }
    }

    public void UpdateGameState(GameState newState)
    {
        Instance.currentState = newState;
        onStateChange?.Invoke(newState);
        Debug.Log(Instance.currentState);
        switch (newState)
        {
            case GameState.StartMenu:
                Instance.startMenu.SetActive(true);
                Instance.pauseObject.SetActive(false);
                Instance.gameOvrMenu.SetActive(false);
                Instance.musicScene.SetActive(false);
                break;
            case GameState.PauseMenu:
                Instance.pauseIsActive = !pauseIsActive;
                Instance.pauseObject.SetActive(pauseIsActive);

                Time.timeScale = pauseIsActive?0:1;
                if (!pauseIsActive)
                {
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
                Instance.numberDeadEnemies = maxEnemies;
                Invoke("Wait", 5);
                break; 
            case GameState.Warning:
                Time.timeScale = 1;
                break; 
            case GameState.Alive:
                Time.timeScale = 1;
                Instance.instrMenu.SetActive(false);
                animator2.SetBool("sleeping", false);
                animator2.SetBool("openDoors", true);
                break;
            case GameState.InstructionsMenu:
                Instance.musicScene.SetActive(true);
                Time.timeScale = 0;
                Instance.startMenu.SetActive(false);
                Instance.instrMenu.SetActive(true);
                Instance.pauseObject.SetActive(false);
                Instance.gameOvrMenu.SetActive(false);
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
        Instance.numberDeadEnemies = Instance.maxEnemies;
        Instance.floorLevel.text = Instance.currentLevel.ToString();
        Instance.enemiesLeft.text = "Enemies Left: " + Instance.numberDeadEnemies.ToString();
        Instance.pauseObject.SetActive(false);
        Instance.gameOvrMenu.SetActive(false);
        Instance.player.transform.position = new Vector3(-0.756f, -1.496f, 0);
        Instance.buildings.transform.position = new Vector3(0.718f, 3.114f, 0);
        Instance.greenBg.transform.position = new Vector3(0.718f, 2.152f, 0);
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject obj in allEnemies)
        {
            Destroy(obj);
        }   
        Instance.UpdateGameState(GameState.Alive);
    }   

    public void home()
    {
        Instance.restartIsPressed();
        Instance.UpdateGameState(GameState.StartMenu);
    }

    public void playGame()
    {
        Instance.UpdateGameState(GameState.InstructionsMenu);
    }
    public void stopGame()
    {
        Destroy(Instance);
        Application.Quit();
    }

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

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
    public Animator animator;
    private bool isActive;
    public Animator animator2;
    private float time = 0;
    private int count = 0;
    private float timer = 0f;
    private float waitTime = 10f;

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
        Instance.UpdateGameState(GameState.InstructionsMenu);
        Instance.floorLevel.text = Instance.currentLevel.ToString();
        Instance.pauseObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.fixedDeltaTime;
        Debug.Log($"{timer}");
        if (timer <= waitTime)
        {
            count++;
            timer -= waitTime + Time.fixedDeltaTime;
            Debug.Log($"{waitTime} lapsed and count is {count}");
        }

        bool stateInstr = currentState == GameState.InstructionsMenu;
        bool statePause = currentState == GameState.PauseMenu;

        if (Instance.numberDeadEnemies == Instance.maxEnemies && !Instance.levelChanged && !statePause)
        {
            Instance.levelChanged = true;
            Instance.UpdateGameState(GameState.LevelTransition);
        }
        if (Instance.numberEnemies >= Instance.capacityEnemies * 0.7 && !statePause && currentState != GameState.Warning)
        {
            Instance.UpdateGameState(GameState.Warning);
        }
        if (Instance.currentState == GameState.Warning && Instance.numberEnemies < Instance.capacityEnemies * 0.7 && !statePause)
        {
            Instance.UpdateGameState(GameState.Alive);
        }
        //if (Instance.numberEnemies == Instance.capacityEnemies && !statePause)
        //{
        //    Instance.UpdateGameState(GameState.GameOver);
        //}
        if (Input.GetKeyDown(KeyCode.Escape) && !stateInstr)
        {
            Instance.UpdateGameState(GameState.PauseMenu);
            isActive = false;
        }
        if (Input.GetKeyDown(KeyCode.P) && stateInstr)
        {
            stateInstr = false;
            Instance.UpdateGameState(GameState.Alive);
            animator2.SetBool("sleeping", false);
        }
        if (isActive)
        {
            Instance.UpdateGameState(GameState.Alive);
        }
       
    }

    public void UpdateGameState(GameState newState)
    {
        Instance.currentState = newState;
        onStateChange?.Invoke(newState);
        //Debug.Log(currentState);
        switch (newState)
        {
            case GameState.StartMenu:
                SceneManager.LoadScene("MainMenu")
                ;
                break;
            case GameState.PauseMenu:
                pauseIsActive = !pauseIsActive;
                pauseObject.SetActive(pauseIsActive);

                Time.timeScale = pauseIsActive?0:1;
                if (!pauseIsActive)
                {
                    isActive = true;
                    currentState = GameState.Alive;
                    onStateChange?.Invoke(currentState);
                }
                else
                    isActive = false;
                break;
            case GameState.LevelTransition:
                Instance.currentLevel++;
                Instance.floorLevel.text = Instance.currentLevel.ToString();
                Instance.maxEnemies = 6 * Instance.currentLevel;
                Instance.numberEnemies = 0;
                Instance.numberDeadEnemies = 0;
                animator2.SetBool("sleeping", false);
                animator2.SetBool("openDoors", false);

                time += Time.deltaTime;
                while(time < 5)
                {
                    time += Time.deltaTime;
                }
                
                if(time >= 4.5) {
                    animator2.SetBool("openDoors", true);
                    Instance.UpdateGameState(GameState.Alive);
                    }
                break; 
            case GameState.Warning:
                break; 
            case GameState.Alive:
                instrMenu.SetActive(false);
                Instance.levelChanged = false;
                break;
            case GameState.InstructionsMenu:
                instrMenu.SetActive(true);
                animator.SetBool("Walk",true);
                break;
            case GameState.GameOver:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }
       
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
        Instance.UpdateGameState(GameState.StartMenu);
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

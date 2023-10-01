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
        Debug.Log(currentState);
        bool stateInstr = currentState == GameState.InstructionsMenu;

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
        if (Input.GetKeyDown(KeyCode.Escape) && !stateInstr)
        {
            Instance.UpdateGameState(GameState.PauseMenu);
            isActive = false;
        }
        if(Instance.numberEnemies == Instance.capacityEnemies)
        {
            Instance.UpdateGameState(GameState.GameOver);
        }
        if (Input.GetKeyDown(KeyCode.P) && stateInstr)
        {
            stateInstr = false;
            Instance.UpdateGameState(GameState.Alive);
        }
        if (isActive)
        {
            Instance.UpdateGameState(GameState.Alive);
        }
           if(currentState != GameState.Alive)
        {
            isActive = false;
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
                /*SceneManager.LoadScene("MainMenu")*/
                ;
                break;
            case GameState.PauseMenu:
                pauseIsActive = !pauseIsActive;
                pauseObject.SetActive(pauseIsActive);
                if (!pauseIsActive)
                {
                    isActive = true;
                    currentState = GameState.Alive;
                }
                break;
            case GameState.LevelTransition:
                Instance.currentLevel++;
                Instance.floorLevel.text = Instance.currentLevel.ToString();
                Instance.maxEnemies = 6 * Instance.currentLevel;
                Instance.numberEnemies = 0;
                Instance.numberDeadEnemies = 0;
                Instance.UpdateGameState(GameState.Alive);
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

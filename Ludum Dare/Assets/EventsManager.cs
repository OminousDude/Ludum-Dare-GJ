using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        GameManager.onStateChange += OnGameStateChange;
    }

    private void OnGameStateChange(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Alive:
                animator.SetBool("sleeping", false);
                break;
        }
    }
}

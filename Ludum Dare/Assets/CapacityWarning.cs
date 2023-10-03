using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapacityWarning : MonoBehaviour
{
    [SerializeField] GameObject warningSign;
    [SerializeField] GameObject redAlert;

    private void Awake()
    {
        GameManager.onStateChange += OnGameStateChange;
    }

    private void OnGameStateChange(GameManager.GameState obj) {
        warningSign.SetActive(obj == GameManager.GameState.Warning);
        redAlert.SetActive(obj == GameManager.GameState.Warning);
    }
}

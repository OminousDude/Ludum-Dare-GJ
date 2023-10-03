using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaximumCapacityScript : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (gameManager.numberEnemies >= gameManager.capacityEnemies)
        {
            Debug.Log("I AM DEAD :(");
        }
    }
}

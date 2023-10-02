using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;

    [SerializeField]
    private GameObject[] enemyPrefabs;

    [SerializeField]
    private float enemyInterval = 1.5f;

    private GameManager gameManager;
    private int spawnedEnemies;

    private void Awake()
    {
        GameManager.onStateChange += OnGameStateChange;
        gameManager = GameManager.Instance;
    }

    private void OnGameStateChange(GameManager.GameState obj) {
        if (obj == GameManager.GameState.Alive)
        {
            if (gameManager.numberEnemies == 0)
            {
                spawnedEnemies = 0;
                StartCoroutine(spawnEnemy(enemyInterval, enemyPrefabs));
            }
        }
    }

    private void OnDestroy()
    {
        GameManager.onStateChange -= OnGameStateChange;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    private IEnumerator spawnEnemy(float interval, GameObject[] enemy)
    {
        if (spawnedEnemies > 0)
        {
            yield return new WaitForSeconds(interval);
        }
        spawnedEnemies++;
        int randSpawnPoint = Random.Range(0, spawnPoints.Length);

        GameObject newEnemy = Instantiate(enemy[Random.Range(0, enemyPrefabs.Length)], spawnPoints[randSpawnPoint].position, Quaternion.identity);

        if (spawnedEnemies < gameManager.maxEnemies)
        {
            StartCoroutine(spawnEnemy(interval, enemy));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
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

    private void Awake()
    {
        GameManager.onStateChange += OnGameStateChange;
    }

    private void OnGameStateChange(GameManager.GameState obj) {
        if (obj == GameManager.GameState.Alive)
        {
            gameManager = GameManager.Instance;
            if (gameManager.numberEnemies == 0)
            {
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
        if (gameManager.numberEnemies > 0)
        {
            yield return new WaitForSeconds(interval);
        }
        gameManager.numberEnemies++;

        int randSpawnPoint = Random.Range(0, spawnPoints.Length);

        GameObject newEnemy = Instantiate(enemy[Random.Range(0, enemyPrefabs.Length)], spawnPoints[randSpawnPoint].position, Quaternion.identity);

        if (gameManager.numberEnemies < gameManager.maxEnemies)
        {
            StartCoroutine(spawnEnemy(interval, enemy));
        }
    }
}

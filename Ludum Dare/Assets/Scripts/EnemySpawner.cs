using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float enemyInterval = 1.5f;

    private GameManager gameManager;

    private int enemyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        StartCoroutine(spawnEnemy(enemyInterval, enemyPrefab));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        gameManager.numberEnemies++;
        enemyCount++;
        if (enemyCount > 0)
        {
            yield return new WaitForSeconds(interval);
        }

        int randSpawnPoint = Random.Range(0, spawnPoints.Length);

        GameObject newEnemy = Instantiate(enemy, spawnPoints[randSpawnPoint].position, Quaternion.identity);

        if (enemyCount < gameManager.maxEnemies)
        {
            StartCoroutine(spawnEnemy(interval, enemy));
        }
    }
}

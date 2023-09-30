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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(enemyInterval, enemyPrefab));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);

        int randSpawnPoint = Random.Range(0, spawnPoints.Length);

        GameObject newEnemy = Instantiate(enemy, spawnPoints[randSpawnPoint].position, Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}

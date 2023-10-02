using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{

    [SerializeField] 
    private float spawnRate = 1f;

    [SerializeField] 
    private GameObject[] cloudPrefabs;

    [SerializeField] 
    private bool canSpawn = true;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Spawner() );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Spawner ()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);

        while (canSpawn)
        {
            yield return wait;
            int rand = Random.Range(0, cloudPrefabs.Length);
            GameObject cloudToSpawn = cloudPrefabs[rand];
            Instantiate(cloudToSpawn, transform.position, Quaternion.identity);
        }
    }
}

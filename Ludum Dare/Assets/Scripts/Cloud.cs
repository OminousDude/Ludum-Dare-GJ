using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float newX = transform.position.x + scrollSpeed * Time.deltaTime;
        float rand = Random.Range(0.1f, 0.7f);
        scrollSpeed = rand;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        if (transform.position.x >= 5)
            OnDestroy();
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}

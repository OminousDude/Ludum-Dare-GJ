using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberEnemies : MonoBehaviour
{
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
            gameManager.numberEnemies++;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
            gameManager.numberEnemies--;
    }

}

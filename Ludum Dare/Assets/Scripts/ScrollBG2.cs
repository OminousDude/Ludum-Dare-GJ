using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBG2 : MonoBehaviour
{
    public float scrollSpeed = 0.25f;
    public float stopPosition = -1f;
    public GameManager gameManager;
    public bool hasLeveledUp = false;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (hasLeveledUp)
        {
            float newY = transform.position.y - scrollSpeed * Time.deltaTime;

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (newY <= stopPosition)
            {
                // Stop scrolling
                hasLeveledUp = false;
                transform.position = new Vector3(transform.position.x, stopPosition, transform.position.z);
                stopPosition = stopPosition - 0.5f;
            }
        }
    }

    public void LevelUp()
    {
        if(gameManager.currentState == GameManager.GameState.LevelTransition)
            hasLeveledUp = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorScroll : MonoBehaviour
{
    [Range(0f, 1f)]
    private float scrollSpeed = 0.2f;
    private float scrollDistance = 0.5f; // Set the distance to stop scrolling.
    private float offset;
    private Material mat;
    private Vector2 initialOffset;
    public bool hasLeveledUp = false;
    private GameManager manager;
    void Start()
    {
        manager = GameManager.Instance;
        mat = GetComponent<Renderer>().material;
        initialOffset = mat.GetTextureOffset("_MainTex");
    }

    void Update()
    {
        if (hasLeveledUp)
        {
            offset += Time.deltaTime * scrollSpeed;
            mat.SetTextureOffset("_MainTex", new Vector2(initialOffset.x, initialOffset.y + offset));

            // Check if we have scrolled the desired distance, and stop scrolling.
            if (offset >= scrollDistance)
            {
                offset = scrollDistance;
                scrollDistance += 1f;
                hasLeveledUp = false; // Stop scrolling.
            }
        }
        isLeveledUp();
    }

    public void isLeveledUp()
    {
        if (manager.currentState == GameManager.GameState.LevelTransition)
            hasLeveledUp = true;
        else
        {
            hasLeveledUp = false;
        }
    }
}

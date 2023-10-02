using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    public float stopPosition = -1.5f;

    public bool hasLeveledUp = false;

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
                stopPosition = stopPosition - 1f;
            }
        }
    }

    public void LevelUp()
    {
        hasLeveledUp = true;
    }
}

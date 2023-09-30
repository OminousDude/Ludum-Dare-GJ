using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemyTarget : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] [Range(0f, 4f)] float lerpTime;

    int posIndex = 0;
    float t = 0f;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, points[posIndex].position, lerpTime * Time.deltaTime);

        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);

        if (t > 0.9f )
        {
            t = 0f;
            posIndex = (posIndex + 1) % points.Length;
        }
    }
}

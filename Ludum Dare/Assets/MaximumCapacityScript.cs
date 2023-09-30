using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaximumCapacityScript : MonoBehaviour
{
    private float max;
    // Start is called before the first frame update
    void Start()
    {
        max = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (max >= 10)
        {
            Debug.Log("WARNING!! TOO MANY");
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        max += 1;
        //Debug.Log(max);
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        max -= 1;
       //Debug.Log(max);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float minWidth;
    public float maxWidht;

    public float minHeight;
    public float maxHeight;

    public int range = 5;

    void Start()
    {
        transform.localScale = new Vector2(Random.Range(minWidth * 100, maxWidht * 100) / 100, Random.Range(minHeight * 100, maxHeight * 100) / 100);
        transform.position = new Vector2((float) Random.Range(-range * 100, range * 100) * 0.01f, 6);
    }

    void Update()
    {
        transform.Translate(0, -transform.localScale.y * 0.35f, 0);

        if (transform.position.y < -6)
        {
            Start();
        }
    }
}

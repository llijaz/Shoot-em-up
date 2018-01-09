using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Sprite[] sprites;

    public int delay;

    int index = 0;

    void Start()
    {

    }

    void Update()
    {
        if (index - delay > sprites.Length - 1)
        {
            Destroy(gameObject);
            return;
        }

        index++;

        if (index < delay)
        {
            return;
        }

        try
        {
            GetComponent<SpriteRenderer>().sprite = sprites[index - delay];
        }
        catch { }
    }
}

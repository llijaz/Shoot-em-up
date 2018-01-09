using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllBullets : MonoBehaviour
{
    public static List<Sprite> Bullets;

    public List<Sprite> bullets;

    private void Awake()
    {
        Bullets = bullets;
    }
}

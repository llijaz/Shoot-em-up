using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEnemySprites : MonoBehaviour
{
    public static EnemyBehaviour[] Enemybehaviour;

    public EnemyBehaviour[] enemysprites;

    private void Awake()
    {
        Enemybehaviour = enemysprites;
    }
}

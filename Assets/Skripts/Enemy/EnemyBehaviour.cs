using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Enemy", menuName = "Custom/Ememy")]
public class EnemyBehaviour : ScriptableObject
{
    public Sprite[] sprites;

    public GameObject enemy;

    public bool randomSpawnPosition = true;

    public bool isBoss;

    public bool canSpawn = true;

    public int probability = 1;

    public int requiredScore = 0;

    public void Summon()
    {
        GameObject go = Instantiate(enemy);

        if (randomSpawnPosition)
            go.transform.position = new Vector2(Random.Range(-250, 250) * 0.01f, 5.5f);
    }
}

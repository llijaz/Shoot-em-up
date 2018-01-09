using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBubbles : Enemy
{
    int bulletTime;
    int ellapsedTime;

    public override EnemyBehaviour Sprites()
    {
        return AllEnemySprites.Enemybehaviour[2];
    }

    public override int Health()
    {
        return 5;
    }

    public override bool Invincible()
    {
        return false;
    }

    public override int Score()
    {
        return 2;
    }

    public override void Init()
    {
        AddBulletBehaviour(new BulletRandomDir());
        GetComponent<SpriteRenderer>().sprite = Sprites().sprites[UnityEngine.Random.Range(0, Sprites().sprites.Length)];
    }

    public override void Move()
    {
        ellapsedTime++;

        transform.Translate(Mathf.Cos(ellapsedTime * 0.08f) * 0.02f, -0.01f, 0);

        bulletTime++;

        if (bulletTime > 10)
        {
            Shoot(Mathf.Cos(ellapsedTime * 0.01f) * 25);

            bulletTime = 0;
        }
    }

    public override void GotHit(int damage)
    {

    }

    public override void Die()
    {

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrone : Enemy
{
    int bulletTime;
    int ellapsedTime;

    public override EnemyBehaviour Sprites()
    {
        return AllEnemySprites.Enemybehaviour[0];
    }

    public override int Health()
    {
        return 11;
    }

    public override bool Invincible()
    {
        return false;
    }

    public override int Score()
    {
        return 5;
    }

    public override void Init()
    {
        AddBulletBehaviour(new BulletTribbleShot());
    }

    public override void Move()
    {
        ellapsedTime++;

        transform.Translate(Mathf.Cos(ellapsedTime * 0.01f) * 0.01f, -0.01f, 0);

        bulletTime++;

        if (bulletTime > 100)
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

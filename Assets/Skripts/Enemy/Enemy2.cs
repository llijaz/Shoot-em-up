using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    int bulletTime;

    public override EnemyBehaviour Sprites()
    {
        return AllEnemySprites.Enemybehaviour[1];
    }

    public override int Health()
    {
        return 25;
    }

    public override bool Invincible()
    {
        return false;
    }

    public override int Score()
    {
        return 10;
    }

    public override void Init()
    {
        AddBulletBehaviour(new BulletMissile());
        AddBulletBehaviour(new BulletUp());
        AddBulletBehaviour(new BulletUp());
        AddBulletBehaviour(new BulletElement());
    }

    public override void Move()
    {
        transform.Translate(0, -0.01f, 0);

        bulletTime++;

        if (bulletTime > 175)
        {
            Shoot();

            bulletTime = 0;
        }
    }

    public override void GotHit(int damage)
    {

    }

    public override void Die()
    {
        bulletBehaviour = new List<BulletBehaviour>();

        AddBulletBehaviour(new BulletPump());

        Shoot();
    }
}

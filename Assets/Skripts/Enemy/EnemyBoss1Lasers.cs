using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss1Lasers : Enemy
{
    int bulletTime;
    int ellapsedTime;

    public override EnemyBehaviour Sprites()
    {
        return AllEnemySprites.Enemybehaviour[4];
    }

    public override int Health()
    {
        return 0;
    }

    public override bool Invincible()
    {
        return true;
    }

    public override int Score()
    {
        return 0;
    }

    public override void Init()
    {
        AddBulletBehaviour(new BulletLaser());
    }

    public override void Move()
    {
        if (transform.position.y > 2.4f)
            return;

        bulletTime++;

        if (bulletTime > 100)
        {
            Shoot(UnityEngine.Random.Range(-20, 20));

            bulletTime = 0;
        }
    }

    public override void GotHit(int damage)
    {
        bulletTime = -500;

        transform.parent.GetComponent<HitFlash>().Hit();

        GameObject explosion = Instantiate(Casual.instance.explosionPrefab);
        explosion.transform.position = transform.position;
    }

    public override void Die()
    {

    }
}

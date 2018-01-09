using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRandomDir : BulletBehaviour
{
    public static float amount;

    public override void Add(Player player, int index)
    {
    }

    public override void Destroy(Bullet bullet, bool hit)
    {
    }

    public override int GetPriority()
    {
        return 2;
    }

    public override void Move(Bullet bullet, Vector2 dir)
    {
    }

    public override void Remove(Player player)
    {
    }

    public override void Start(Bullet bullet)
    {
        bullet.transform.Rotate(0, 0, UnityEngine.Random.Range(-20, 20));

        bullet.GetComponent<SpriteRenderer>().sprite = AllBullets.Bullets[44];

        bullet.damage = 5;
    }

    public override void Update(Bullet bullet)
    {
    }
}

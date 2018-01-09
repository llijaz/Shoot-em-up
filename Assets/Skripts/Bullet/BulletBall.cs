using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBall : BulletBehaviour
{
    public override void Add(Player player, int index)
    {
    }

    public override void Destroy(Bullet bullet, bool hit)
    {
    }

    public override int GetPriority()
    {
        return 4;
    }

    public override void Move(Bullet bullet, Vector2 dir)
    {
    }

    public override void Remove(Player player)
    {
    }

    public override void Start(Bullet bullet)
    {
        if (bullet.type != BulletType.Beam)
            return;

        bullet.type = BulletType.Ball;

        bullet.damage = (int) (bullet.damage * 1.2f);
    }

    public override void Update(Bullet bullet)
    {
    }
}

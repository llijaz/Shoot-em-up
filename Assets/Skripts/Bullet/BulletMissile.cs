using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMissile : BulletBehaviour
{
    public override void Add(Player player, int index)
    {
        player.shootDelay[index] = (int)(player.shootDelay[index] * 1.4f);
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

        bullet.type = BulletType.Missile;

        bullet.damage = (int) (bullet.damage * 1.5f);
    }

    public override void Update(Bullet bullet)
    {
    }
}

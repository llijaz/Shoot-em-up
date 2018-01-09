using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWave : BulletBehaviour
{
    public override void Add(Player player, int index)
    {
        player.shootDelay[index] = (int)(player.shootDelay[index] * 0.9f);
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

        bullet.type = BulletType.Wave;
    }

    public override void Update(Bullet bullet)
    {
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRotateToRight : BulletBehaviour
{
    public static int amount;

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
        RemoveComponentFromPlayer(bullet);

        bullet.transform.Rotate(0, 0, amount);
    }

    public override void Update(Bullet bullet)
    {
    }
}

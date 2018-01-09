using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRespeed : BulletBehaviour
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
        bullet.speed = amount;

        RemoveComponentFromPlayer(bullet);
    }

    public override void Update(Bullet bullet)
    {
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletElement : BulletBehaviour
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
        bullet.element = !bullet.element;

        bullet.damage += 1;
    }

    public override void Update(Bullet bullet)
    {
    }
}

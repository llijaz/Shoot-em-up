using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDontMove : BulletBehaviour
{
    public override void Add(Player player, int index)
    {
    }

    public override void Destroy(Bullet bullet, bool hit)
    {
    }

    public override int GetPriority()
    {
        return 0;
    }

    public override void Move(Bullet bullet, Vector2 dir)
    {
        bullet.@break = true;
        bullet.canMove = false;
    }

    public override void Remove(Player player)
    {
    }

    public override void Start(Bullet bullet)
    {
    }

    public override void Update(Bullet bullet)
    {
    }
}

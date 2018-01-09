using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSize : BulletBehaviour
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
        bullet.speed *= 0.9f;

        bullet.damage = (int)(bullet.damage * 1.4f);
        bullet.transform.localScale = new Vector3(bullet.transform.localScale.x * 1.1f, bullet.transform.localScale.y * 1.1f, 1);
    }

    public override void Update(Bullet bullet)
    {
    }
}

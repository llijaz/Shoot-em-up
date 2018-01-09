using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTribbleShot : BulletBehaviour
{
    public override void Add(Player player, int index)
    {
    }

    public override void Destroy(Bullet bullet, bool hit)
    {
    }

    public override int GetPriority()
    {
        return 3;
    }

    public override void Move(Bullet bullet, Vector2 dir)
    {
    }

    public override void Remove(Player player)
    {
    }
    
    public override void Start(Bullet bullet)
    {
        List<BulletBehaviour> list = GetList(bullet);

        for (int i = 1; i < list.Count; i++)
        {
            if ((list[i].GetType().Name.Equals(new BulletPump().GetType().Name)
            || list[i].GetType().Name.Equals(new BulletTribbleShot().GetType().Name)
            || list[i].GetType().Name.Equals(new BulletDoubleShot().GetType().Name))
            && list[i] != this)
            {
                list.RemoveAt(i);
                i--;
            }
        }

        GameObject go;

        go = SpawnBullet(bullet, this);
        go.transform.Rotate(0, 0, 10);
        go.GetComponent<Bullet>().Init();

        go = SpawnBullet(bullet, this);
        go.transform.Rotate(0, 0, -10);
        go.GetComponent<Bullet>().Init();
    }

    public override void Update(Bullet bullet)
    {
    }
}

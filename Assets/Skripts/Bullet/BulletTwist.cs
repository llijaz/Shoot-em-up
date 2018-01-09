using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTwist : BulletBehaviour
{
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
        List<BulletBehaviour> list = GetList(bullet);

        for (int i = 0; i < list.Count; i++)
        {
            if ((list[i].GetType().Name.Equals(new BulletTwist().GetType().Name) || list[i].GetType().Name.Equals(new BulletLaser().GetType().Name)) && list[i] != this)
            {
                list.RemoveAt(i);
                i--;
            }
        }

        bullet.GetComponent<SpriteRenderer>().sprite = AllBullets.Bullets[93];

        GameObject child = new GameObject();
        child.name = "Rotate childs";
        child.transform.parent = bullet.transform;
        child.transform.localPosition = new Vector3(0, 0, 0);
        child.transform.localScale = new Vector3(1, 1, 1);

        GameObject[] bullets = new GameObject[4];
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = SpawnBullet(bullet, this);

            bullets[i].transform.parent = child.transform;
            bullets[i].transform.Translate(new float[] { 0.5f, 0, -0.5f, 0 }[i], new float[] { 0, 0.5f, 0, -0.5f }[i], 0);
            bullets[i].transform.Rotate(0, 0, new float[] { 0, 90, 180, 270 }[i]);
            bullets[i].GetComponent<Bullet>().Init();
            bullets[i].GetComponent<Bullet>().behaviour.Add(new BulletDontMove());
        }
    }

    public override void Update(Bullet bullet)
    {
        try
        {
            bullet.transform.GetChild(0).transform.Rotate(0, 0, 5);
        }
        catch { }
    }
}

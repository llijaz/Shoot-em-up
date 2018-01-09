using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGrenate : BulletBehaviour
{
    public override void Add(Player player, int index)
    {
    }

    public override void Destroy(Bullet bullet, bool hit)
    {
    }

    public override int GetPriority()
    {
        return 1;
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
            if (list[i].GetType().Name.Equals(new BulletGrenate().GetType().Name) && list[i] != this)
            {
                list.RemoveAt(i);
                i--;
            }
        }

        bullet.GetComponent<SpriteRenderer>().sprite = AllBullets.Bullets[41];

        bullet.@break = true;
    }

    public override void Update(Bullet bullet)
    {
        float y = bullet.transform.position.y;

        if (y < 1 && y > -1)
        {
            // Spawn bullet

            GameObject go;

            int[] dirs = new int[] { -45, 0, 45 };

            // is there a pump behavoiour

            List<BulletBehaviour> list = GetList(bullet);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].GetType().Name.Equals(new BulletPump().GetType().Name))
                {
                    dirs = new int[] { 0 };
                    break;
                }
            }

            foreach (int r in dirs)
            {
                go = SpawnBullet(bullet, this);
                go.transform.Rotate(0, 0, r);
                go.GetComponent<Bullet>().Init();
            }

            go = GameObject.Instantiate(Game.instance.explosionPrefab);
            go.GetComponent<AudioSource>().enabled = false;
            go.transform.position = bullet.transform.position;

            GameObject.Destroy(bullet.gameObject);
            // bullet.transform.position = new Vector2(100, 0);
        }

    }
}

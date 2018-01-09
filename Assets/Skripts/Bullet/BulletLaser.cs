using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLaser : BulletBehaviour
{
    int textureIndex;

    bool hitSth;

    float ellapsedTime;
    int flashTime;

    public override void Add(Player player, int index)
    {
        for (int i = 0; i < player.bulletBehaviour[index].Count; i++)
        {
            if (player.bulletBehaviour[index][i].GetType().Name.Equals(new BulletTwist().GetType().Name))
            {
                player.bulletBehaviour[index].RemoveAt(i);
                i--;
            }
        }
    }

    public override void Destroy(Bullet bullet, bool hit)
    {
        if (bullet.type != BulletType.Laser)
            return;

        hitSth = true;

        Bullet.destroy = false;
        GameObject.Destroy(bullet.gameObject, 2);

        bullet.gameObject.GetComponent<HitFlash>().Hit();
    }

    public override int GetPriority()
    {
        return 4;
    }

    public override void Move(Bullet bullet, Vector2 dir)
    {
        if (bullet.type == BulletType.Laser)
            bullet.canMove = false;
    }

    public override void Remove(Player player)
    {
    }

    public override void Start(Bullet bullet)
    {
        if (bullet.type != BulletType.Beam)
            return;

        List<BulletBehaviour> list = GetList(bullet);

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].GetType().Name.Equals(new BulletLaser().GetType().Name) && list[i] != this)
            {
                list.RemoveAt(i);
                i--;
            }
        }

        textureIndex = 0;
        hitSth = false;
        ellapsedTime = 0;

        bullet.damage *= 2;

        bullet.gameObject.AddComponent<HitFlash>();

        bullet.type = BulletType.Laser;

        bullet.GetComponent<SpriteRenderer>().sprite = AllBullets.Bullets[45 + 12 * bullet.level + (bullet.element ? 6 : 0)];

        bullet.@break = true;

        GameObject.Destroy(bullet.gameObject, 3);
    }

    public override void Update(Bullet bullet)
    {
        if (bullet.type != BulletType.Laser)
            return;

        // set texture

        if (textureIndex < 5)
        {
            bullet.GetComponent<SpriteRenderer>().sprite = AllBullets.Bullets[45 + 12 * bullet.level + (bullet.element ? 6 : 0) + textureIndex];

            textureIndex++;
        }

        if (!hitSth)
        {
            try
            {
                bullet.source.GetComponent<Player>().shootDelayTime = 0;
            }
            catch { }
        }

        ellapsedTime += Time.deltaTime * 100;

        if (ellapsedTime > 120 || hitSth)
        {
            bullet.transform.Translate(0, 0.5f * bullet.dir.y, 0);
        }

        bullet.transform.Translate(0, 0.5f * bullet.dir.y, 0);
        bullet.GetComponent<SpriteRenderer>().size += new Vector2(0, 0.14f);
        bullet.GetComponent<BoxCollider2D>().size += new Vector2(0, 0.14f);

        if (hitSth)
        {
            flashTime++;
            if (flashTime > 30)
            {
                flashTime = 0;
                bullet.gameObject.GetComponent<HitFlash>().Hit();
            }
        }
    }
}

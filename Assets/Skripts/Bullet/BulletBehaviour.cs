using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBehaviour
{
    private int index;

    internal List<BulletBehaviour> GetList(Bullet bullet)
    {
        List<BulletBehaviour> list = null;

        if (bullet.source.tag == "Player")
            list = bullet.source.GetComponent<Player>().bulletBehaviour[bullet.bulletIndex];

        if (bullet.source.tag == "Enemy")
            list = bullet.source.GetComponent<Enemy>().bulletBehaviour;

        return list;
    }

    internal void RemoveComponentFromPlayer(Bullet bullet)
    {
        List<BulletBehaviour> list = GetList(bullet);

        index = list.IndexOf(this);
        list.Remove(this);
    }

    internal void AddComponentToPlayer(Bullet bullet)
    {
        List<BulletBehaviour> list = GetList(bullet);

        list.Insert(index, this);
    }

    internal void AddComponentToPlayer(BulletBehaviour behaviour, Bullet bullet)
    {
        if (bullet.source.tag == "Player")
            bullet.source.GetComponent<Player>().AddBulletBehaviour(bullet.bulletIndex, new BulletRotateToRight());

        if (bullet.source.tag == "Enemy")
            bullet.source.GetComponent<Enemy>().AddBulletBehaviour(new BulletRotateToRight());
    }

    internal void Shoot(Bullet bullet)
    {
        if (bullet.source.tag == "Player")
            bullet.source.GetComponent<Player>().Shoot(bullet.bulletIndex, true);

        if (bullet.source.tag == "Enemy")
            bullet.source.GetComponent<Enemy>().Shoot();
    }

    internal GameObject SpawnBullet(Bullet bullet, BulletBehaviour behaviour)
    {
        List<BulletBehaviour> list = bullet.behaviour;

        List<BulletBehaviour> newList = new List<BulletBehaviour>();

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != behaviour)
                newList.Add(list[i]);
        }

        GameObject go = GameObject.Instantiate(Game.BulletPrefab);

        go.transform.position = bullet.transform.position;
        go.transform.rotation = bullet.transform.rotation;

        if (bullet.dir.y < 0)
        {
            go.GetComponent<SpriteRenderer>().flipY = true;
        }

        go.GetComponent<Bullet>().behaviour = newList;

        go.GetComponent<Bullet>().dir = bullet.dir;
        go.GetComponent<Bullet>().bulletIndex = bullet.bulletIndex;
        go.GetComponent<Bullet>().source = bullet.source;

        return go;
    }

    public abstract int GetPriority();

    public abstract void Start(Bullet bullet);
    public abstract void Update(Bullet bullet);
    public abstract void Move(Bullet bullet, Vector2 dir);
    public abstract void Destroy(Bullet bullet, bool hit);

    public abstract void Add(Player player, int index);
    public abstract void Remove(Player player);
}

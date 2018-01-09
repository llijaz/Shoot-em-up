using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Beam, Ball, Wave, Missile, Laser
}

public class Bullet : MonoBehaviour
{
    public static bool destroy;

    public Transform source;

    public Vector2 dir;

    public float speed = 0.05f;

    public int damage = 10;

    public bool canMove { get; set; }

    public List<BulletBehaviour> behaviour = new List<BulletBehaviour>();

    public BulletType type;

    public int level;

    public bool element;

    public int bulletIndex;

    public bool @break;

    public void Init()
    {
        if (behaviour == null)
        {
            behaviour = new List<BulletBehaviour>();
        }

        level = 0;
        element = false;

        // Destroy(transform.gameObject, 5);

        GetComponent<SpriteRenderer>().sprite = null;

        // execute behaviour.Start()

        for (int p = 0; p < 5; p++)
        {
            for (int i = 0; i < behaviour.Count; i++)
            {
                if (behaviour[i].GetPriority() == p)
                {
                    behaviour[i].Start(this);

                    if (@break)
                        break;
                }
            }

            if (@break)
            {
                @break = false;
                break;
            }
        }

        if (GetComponent<SpriteRenderer>().sprite == null)
            SetSprite(AllBullets.Bullets[(int)type * 10 + level + (element ? 5 : 0)]);

        Destroy(gameObject, 3);
    }

    void Update()
    {
        // execute behaviour.Update()

        for (int p = 0; p < 5; p++)
        {
            for (int i = 0; i < behaviour.Count; i++)
            {
                if (behaviour[i].GetPriority() == p)
                {
                    behaviour[i].Update(this);
                }
            }
        }

        // move

        canMove = true;
        @break = false;

        // execute behaviour.Move()

        for (int p = 0; p < 5; p++)
        {
            for (int i = 0; i < behaviour.Count; i++)
            {
                if (behaviour[i].GetPriority() == p)
                {
                    behaviour[i].Move(this, dir);
                }

                if (@break)
                    break;
            }

            if (@break)
            {
                @break = false;
                break;
            }
        }

        if (canMove)
            transform.Translate(dir.x * speed * Time.deltaTime * 100, dir.y * speed * Time.deltaTime * 100, 0);

        if (speed < 0.01f)
            speed = 0.01f;

        if (speed > 0.1f)
            speed = 0.1f;
    }

    public void SetSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void AddLevel(int i)
    {
        level += i;
        if (level > 3)
        {
            level = 3;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    /*
        if (Main.mode == Mode.Join)
        {
            return;
        }

        Transform target = collision.transform;

        if (target == source)
            return;

        if (target.tag != "Player")
        {
            // hit sth else

            if (target.tag == "Item")
            {
                // if someone else is hitting a item

                try
                {
                    if (source == null || source.tag != "Player")
                        return;
                }
                catch
                {
                    return;
                }

                if (Main.mode == Mode.Host)
                {
                    Game.instance.network.Write("f " + target.name + ":" + damage + ":" + source.GetComponent<Player>().GetPlayerID());
                }

                target.GetComponent<Item>().Hit(damage, source.GetComponent<Player>().GetPlayerID());

                destroy = true;
                for (int i = 0; i < behaviour.Count; i++)
                {
                    behaviour[i].Destroy(this, true);
                }

                if (destroy)
                    Destroy();
            }

            if (target.tag == "Enemy")
            {
                target.GetComponent<Enemy>().Hit(damage);

                destroy = true;
                for (int i = 0; i < behaviour.Count; i++)
                {
                    behaviour[i].Destroy(this, true);
                }

                if (destroy)
                    Destroy();
            }

            if (target.tag == "Bullet")
            {
                if (target.GetComponent<Bullet>().source == source)
                {
                    return;
                }

                if (target.GetComponent<Bullet>().level <= level)
                {
                    destroy = true;
                    for (int i = 0; i < target.GetComponent<Bullet>().behaviour.Count; i++)
                    {
                        target.GetComponent<Bullet>().behaviour[i].Destroy(this, true);
                    }

                    if (destroy)
                        target.GetComponent<Bullet>().Destroy();
                }

                if (level <= target.GetComponent<Bullet>().level)
                {
                    destroy = true;
                    for (int i = 0; i < behaviour.Count; i++)
                    {
                        behaviour[i].Destroy(this, true);
                    }

                    if (destroy)
                        Destroy();
                }
            }

            return;
        }

        // player hit

        if (Main.mode == Mode.Host)
        {
            Game.instance.network.Write("f " + target.name + ":" + damage + ":" + source.GetComponent<Player>().GetPlayerID());
        }

        target.GetComponent<Player>().Damage(damage);

        destroy = true;
        for (int i = 0; i < behaviour.Count; i++)
        {
            behaviour[i].Destroy(this, true);
        }

        if (destroy)
            Destroy();
            */
}

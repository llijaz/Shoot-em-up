using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static int ID;

    public float speed = 0.01f;

    public int health = 100;

    public ItemBehaviour behaviour;

    int ellapsedTime;

    int dirX;

    Vector3 targetScale;

    bool waiting;

    private void Start()
    {
        transform.name = "Item" + ID;

        ID++;
    }

    public void Init()
    {
        int behaviourID = Random.Range(0, AllItems.Items.Count);

        int dirX = (int)Random.Range(0, 2);
        if (dirX > 0)
            dirX = 1;
        else
            dirX = -1;

        if (Main.mode != Mode.Casual)
            Init(behaviourID, Random.Range(0, 4) * dirX, Random.Range(2, -2), dirX);
        else
            Init(behaviourID, Random.Range(-3, 3) * dirX, Random.Range(-4, 0), dirX);
    }

    public void Init(int behaviourID, float x, float y, int dir)
    {
        behaviour = AllItems.Items[behaviourID];

        GetComponent<SpriteRenderer>().sprite = behaviour.sprite;

        transform.position = new Vector2(Random.Range(0, 3) * dirX, Random.Range(1, -1));

        targetScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 1);

        dirX = dir;

        if (Main.mode == Mode.Host)
            Game.instance.network.Write("e " + behaviourID + ":" + transform.position.x + ":" + -transform.position.y + ":" + -dirX);
    }

    void Update()
    {
        if (waiting)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 0, 1), 0.001f);

            if (transform.localScale.x < 0.1f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            ellapsedTime += dirX;

            if (Main.mode == Mode.Casual)
                transform.Translate(Mathf.Sin(ellapsedTime * 0.01f) / 100, -speed, 0);
            else
                transform.Translate(dirX * speed, Mathf.Sin(ellapsedTime * 0.01f) / 100, 0);

            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 0.025f);
        }
    }

    public void Hit(int damage, int playerID)
    {
        if (waiting)
            return;

        health -= damage;

        if (health < 0)
        {
            // Add Item to player

            if (behaviour.applyToBullet && Main.mode != Mode.Casual)
            {
                if (playerID == 1)
                {
                    Game.instance.player1.NewBulletBehaviour(this, transform.position);
                }
                else
                {
                    Game.instance.player2.NewBulletBehaviour(this, transform.position);
                }

                Destroy(transform.gameObject);
            }
            else
            {
                waiting = true;

                transform.localScale = new Vector3(targetScale.x * 1.2f, targetScale.y * 1.2f, 1f);
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(transform.gameObject);
    }

    private void OnMouseDown()
    {
        if (waiting)
        {
            ItemEvents.i = 0;
            ItemEvents.player = Casual.instance.player;

            behaviour.trigger.Invoke();

            waiting = false;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            if (Main.mode == Mode.Join)
                return;

            Transform target = collision.transform;

            if (target.tag == "Bullet")
            {
                Bullet bullet = target.GetComponent<Bullet>();

                Hit(bullet.damage, bullet.source.GetComponent<Player>().GetPlayerID());

                if (Main.mode == Mode.Host)
                    Game.instance.network.Write("f " + name + ":" + bullet.damage + ":" + bullet.source.GetComponent<Player>().GetPlayerID());

                Bullet.destroy = true;
                for (int i = 0; i < bullet.behaviour.Count; i++)
                {
                    bullet.behaviour[i].Destroy(bullet, true);
                }

                if (Bullet.destroy)
                    bullet.Destroy();
            }
        }
        catch { }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public string playerName;

    [Space]
    [Header("Stats")]

    public float speed;
    public float movementspeed = 0.5f;

    public int[] shootDelay;

    public int health = 100;
    public int maxHealth = 100;

    public float shield = 100;
    public int maxShield = 100;

    public int shieldReloadDelay = 50;
    public float shieldReloadSpeed = 10;

    public int spray;

    public int buttonCount;

    public GameObject itemButton;

    // end inpector view

    public List<BulletBehaviour>[] bulletBehaviour;

    public HealthBarChange healthBarChange;

    public Sprite[] sprites { get; set; }

    public int shootDelayTime { get; set; }

    int shieldReloadTime;

    float lastDirX;
    float vel;

    public float targetX { get; set; }

    void Start()
    {

    }

    public void Init()
    {
        shootDelay = new int[]
        {
            30, 30, 30
        };

        // disable buttons who are not needed

        if (Main.mode != Mode.Casual)
        {
            int p = GetPlayerID();

            if (buttonCount < 3)
                Destroy(GameObject.Find("Player " + p + " Shoot 3"));
            if (buttonCount < 2)
                Destroy(GameObject.Find("Player " + p + " Shoot 2"));

        }

        bulletBehaviour = new List<BulletBehaviour>[3];
        for (int i = 0; i < bulletBehaviour.Length; i++)
        {
            bulletBehaviour[i] = new List<BulletBehaviour>();
        }

        // DEBUG

    }

    void Update()
    {
        shootDelayTime++;

        // move

        if ((Main.mode == Mode.Host || Main.mode == Mode.Join) && GetPlayerID() == 1)
        {
            float f = Mathf.Round(lastDirX * 100f) / 100f;

            // if (lastDirX != 0)
            // Game.instance.network.Write("k " + -f);
        }

        if (Main.mode == Mode.Join)
        {
            // the host sets the target x position

            lastDirX = targetX - transform.position.x;

            if (lastDirX > 1)
                lastDirX = 1;

            if (lastDirX < -1)
                lastDirX = -1;
        }

        vel = Mathf.Lerp(vel, lastDirX, movementspeed);

        if (Main.mode != Mode.Join)
            transform.Translate(vel * speed * Time.deltaTime * 100, 0, 0);
        // transform.Translate(vel * speed, 0, 0);

        // set player sprite to movement

        int i = 2;

        if (vel > 0.2f)
            i = 3;

        if (vel < -0.2f)
            i = 1;

        if (vel > 0.6f)
            i = 4;

        if (vel < -0.6f)
            i = 0;

        GetComponent<SpriteRenderer>().sprite = sprites[i];

        // regenerating shield

        if (shieldReloadTime > 0)
        {
            shieldReloadTime--;
        }
        else
        {
            if (shield < maxShield)
            {
                shield += shieldReloadSpeed;

                if (healthBarChange != null)
                    healthBarChange.OnHealthChange();

                if (shield > maxShield)
                {
                    shield = maxShield;
                }
            }
        }

        for (int j = 0; j < 3; j++)
        {
            if (shootDelay[j] > 100)
                shootDelay[j] = 100;

            if (shootDelay[j] < 10)
                shootDelay[j] = 10;
        }
    }

    // collision

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

                if (bullet.source.GetComponent<Player>() == this)
                    return;

                if (Main.mode == Mode.Host)
                    Game.instance.network.Write("f " + name + ":" + bullet.damage + ":" + bullet.source.GetComponent<Player>().GetPlayerID());

                Damage(bullet.damage);

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

    public void Move(float dir)
    {
        if (Main.mode == Mode.Join && dir != 0)
            Game.instance.network.Write("k " + -dir);

        lastDirX = dir;
        targetX += dir * speed * Time.deltaTime * 100;
    }

    public void Shoot(int i)
    {
        Shoot(i, false);
    }

    public void Shoot(int i, bool force)
    {
        int p = GetPlayerID();

        if (p == 1)
            Shoot(i, 1, force);
        else
            Shoot(i, -1, force);
    }

    public void Shoot(int i, int dir)
    {
        Shoot(i, dir, false);
    }

    public void Shoot(int i, int dir, bool force)
    {
        if (shootDelayTime >= shootDelay[i] || force)
        {
            try
            {
                GetComponents<AudioSource>()[1].Play();
            }
            catch { }

            shootDelayTime = 0;

            if (Main.mode == Mode.Host && GetPlayerID() == 1)
                Game.instance.network.Write("d " + i);

            if (Main.mode == Mode.Host && GetPlayerID() == 2)
                Game.instance.network.Write("c " + i);

            // Spawn bullet

            GameObject bullet = Instantiate(Game.BulletPrefab);

            bullet.transform.position = transform.position;

            if (dir < 0)
            {
                bullet.GetComponent<SpriteRenderer>().flipY = true;
            }

            bullet.GetComponent<Bullet>().behaviour = bulletBehaviour[i];

            bullet.transform.Rotate(0, 0, lastDirX * -spray);

            bullet.GetComponent<Bullet>().dir = new Vector2(0, dir);
            bullet.GetComponent<Bullet>().bulletIndex = i;
            bullet.GetComponent<Bullet>().source = transform;
            bullet.GetComponent<Bullet>().Init();
        }
    }

    public void Damage(int dmg)
    {
        shieldReloadTime = shieldReloadDelay;

        GetComponent<HitFlash>().Hit();

        if (shield > 0)
        {
            shield -= dmg;
        }
        else
        {
            health -= dmg;

            if (health <= 0)
            {
                Die();
            }
        }

        if (healthBarChange != null)
        {
            healthBarChange.OnHealthChange();
        }
    }

    public void Die()
    {
        GameObject explosion = Instantiate(Game.instance.hugeExplosionPrefab);
        explosion.transform.position = transform.position;

        if (Main.mode == Mode.Casual)
        {
            Casual.instance.Die();
        }

        if (Main.mode == Mode.FreeGame)
        {
            string otherPlayerName = GetOtherPlayer().playerName;

            Game.instance.Win(otherPlayerName);

            transform.gameObject.active = false;
        }
    }

    public void NewBulletBehaviour(Item item, Vector2 target)
    {
        // set the canvas position by the world position
        RectTransform CanvasRect = Game.instance.canvas.GetComponent<RectTransform>();

        itemButton.transform.position = Game.instance.camera.WorldToViewportPoint(target);
        itemButton.transform.position = new Vector2(itemButton.transform.position.x * CanvasRect.sizeDelta.x, itemButton.transform.position.y * CanvasRect.sizeDelta.y);

        // set other vars

        itemButton.GetComponent<Image>().sprite = item.behaviour.sprite;
        itemButton.GetComponent<ItemSelector>().item = item.behaviour;
    }

    public void AddBulletBehaviour(int index, BulletBehaviour behaviour)
    {
        behaviour.Add(this, index);

        bulletBehaviour[index].Insert(0, behaviour);
    }

    Player GetOtherPlayer()
    {
        if (Game.instance.player1.playerName == playerName)
        {
            return Game.instance.player2;
        }

        return Game.instance.player1;
    }

    public int GetPlayerID()
    {
        try
        {
            int p = 1;

            if (playerName == Game.instance.player2.playerName)
                p = 2;

            return p;
        }
        catch { }

        return 1;
    }

}

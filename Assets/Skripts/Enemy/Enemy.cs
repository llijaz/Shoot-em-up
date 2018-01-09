using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int health { get; set; }

    public bool die { get; set; }

    public List<BulletBehaviour> bulletBehaviour;

    bool visible = true;

    void Start()
    {
        health = Health();

        GetComponent<SpriteRenderer>().sprite = Sprites().sprites[0];

        bulletBehaviour = new List<BulletBehaviour>();

        Init();
    }

    void Update()
    {
        Move();

        if (transform.position.y < -7)
        {
            Destroy(gameObject);
        }
    }

    public void Shoot()
    {
        if (!visible)
            return;

        Shoot(0);
    }

    public void Shoot(float rotation)
    {
        // Spawn bullet

        GameObject bullet = Instantiate(Game.BulletPrefab);

        bullet.transform.position = transform.position;

        bullet.GetComponent<SpriteRenderer>().flipY = true;

        bullet.GetComponent<Bullet>().level = 0;
        bullet.GetComponent<Bullet>().element = false;
        bullet.GetComponent<Bullet>().behaviour = bulletBehaviour;

        bullet.transform.Rotate(0, 0, rotation);

        bullet.GetComponent<Bullet>().dir = new Vector2(0, -1);
        bullet.GetComponent<Bullet>().bulletIndex = 0;
        bullet.GetComponent<Bullet>().source = transform;
        bullet.GetComponent<Bullet>().Init();
    }

    public void Hit(int damage)
    {
        if (die)
            return;

        GotHit(damage);

        GetComponent<HitFlash>().Hit();

        health -= damage;

        if (health <= 0)
        {
            if (!Invincible())
                die = true;

            Die();
        }

        if (die)
        {
            GameObject explosion = Instantiate(Casual.instance.explosionPrefab);
            explosion.transform.position = transform.position;

            Casual.instance.score += Score();

            Destroy(gameObject);
        }
    }

    public void AddBulletBehaviour(BulletBehaviour behaviour)
    {
        bulletBehaviour.Add(behaviour);
    }

    private void OnBecameInvisible()
    {
        visible = false;
    }

    private void OnBecameVisible()
    {
        visible = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Main.mode == Mode.Join)
            return;

        Transform target = collision.transform;

        if (target.tag == "Bullet")
        {
            Bullet bullet = target.GetComponent<Bullet>();

            try
            {
                if (bullet.source.tag == "Enemy")
                    return;
            }
            catch { }

            Hit(bullet.damage);

            Bullet.destroy = true;
            for (int i = 0; i < bullet.behaviour.Count; i++)
            {
                bullet.behaviour[i].Destroy(bullet, true);
            }

            if (Bullet.destroy)
                bullet.Destroy();
        }
    }

    public abstract EnemyBehaviour Sprites();
    public abstract int Health();
    public abstract bool Invincible();
    public abstract int Score();

    public abstract void Init();
    public abstract void Move();
    public abstract void GotHit(int damage);

    public abstract void Die();
}

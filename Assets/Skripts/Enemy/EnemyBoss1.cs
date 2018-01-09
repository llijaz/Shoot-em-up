using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoss1 : Enemy
{
    int bulletTime;
    int ellapsedTime;

    int mode;
    int modeTime;

    int deathTime;

    public override EnemyBehaviour Sprites()
    {
        return AllEnemySprites.Enemybehaviour[3];
    }

    public override int Health()
    {
        return 5000;
    }

    public override bool Invincible()
    {
        return true;
    }

    public override int Score()
    {
        return 100;
    }

    public override void Init()
    {
        mode = 1;

        GameObject go;

        go = GameObject.Instantiate(AllEnemySprites.Enemybehaviour[4].enemy);
        go.transform.parent = transform;
        go.transform.localPosition = new Vector2(-0.6f, -0.2f);

        go = GameObject.Instantiate(AllEnemySprites.Enemybehaviour[4].enemy);
        go.transform.parent = transform;
        go.transform.localPosition = new Vector2(0.6f, -0.2f);

        GameObject.Find("Main Camera").GetComponent<AudioSource>().Stop();
        GetComponents<AudioSource>()[1].Play();

        Casual.instance.autoShoot = false;
    }

    public override void Move()
    {
        ellapsedTime++;

        if (transform.position.y > 2.5f)
        {
            transform.Translate(0, -0.01f, 0);
            return;
        }

        Casual.instance.autoShoot = true;

        modeTime++;

        if (modeTime > 300)
        {
            modeTime = 0;
            mode = UnityEngine.Random.Range(1, 4);
            bulletTime = -50;

            bulletBehaviour = new List<BulletBehaviour>();

            switch (mode)
            {
                case 1:
                    break;

                case 2:
                    AddBulletBehaviour(new BulletBall());
                    AddBulletBehaviour(new BulletUp());
                    AddBulletBehaviour(new BulletGrenate());
                    break;

                case 3:
                    AddBulletBehaviour(new BulletLaser());
                    break;
            }
        }

        if (health > 0)
        {
            switch (mode)
            {
                case 1:
                    bulletTime++;

                    if (bulletTime > 10)
                    {
                        Shoot(Mathf.Cos(ellapsedTime * 0.01f) * 25);

                        bulletTime = 0;
                    }
                    break;

                case 2:
                    bulletTime++;

                    if (bulletTime == 50)
                        Shoot();

                    if (bulletTime == 60)
                        Shoot(Mathf.Cos(ellapsedTime * 0.1f) * 25);


                    if (bulletTime >= 70)
                    {
                        Shoot(Mathf.Cos(ellapsedTime * 0.1f) * 25);

                        bulletTime = 0;
                    }
                    break;

                case 3:
                    bulletTime++;
                    modeTime++;

                    if (bulletTime > 50)
                    {
                        Shoot(Mathf.Cos(ellapsedTime * 0.01f) * 25);

                        bulletTime = 0;
                    }
                    break;
            }
        }

        if (health < 2000)
        {
            transform.Translate(Mathf.Cos(ellapsedTime * 0.005f) * 0.01f, 0, 0);
            transform.Rotate(0, 0, -Mathf.Cos(ellapsedTime * 0.005f) * 0.05f);
        }

        if (health < 0)
        {
            Casual.instance.autoShoot = false;

            deathTime++;

            if (deathTime % 20 == 0 && deathTime < 350)
            {
                GameObject explosion = Instantiate(Game.instance.hugeExplosionPrefab);
                explosion.transform.position = transform.position + new Vector3(UnityEngine.Random.Range(-200, 200) * 0.01f, UnityEngine.Random.Range(-100, 100) * 0.01f, -1);
            }

            if (deathTime == 300)
            {
                GetComponents<AudioSource>()[2].Play();
            }

            if (deathTime > 300 && deathTime < 400)
            {
                GameObject.Find("Fade").GetComponent<Image>().color = new Color(1, 1, 1, (deathTime - 300) * 0.1f);
            }

            if (deathTime > 400 && deathTime < 500)
            {
                GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                GameObject.Find("Fade").GetComponent<Image>().color = new Color(1, 1, 1, 1 - (deathTime - 400) * 0.1f);
            }

            if (deathTime > 500)
            {
                GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
                Casual.instance.autoShoot = true;
                Casual.instance.boss = false;
                Casual.instance.spawn = true;
                Destroy(gameObject);
            }

        }

    }

    public override void GotHit(int damage)
    {

    }

    public override void Die()
    {

    }
}

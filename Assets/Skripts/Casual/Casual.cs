using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Casual : MonoBehaviour
{
    public static Casual instance;

    private void Awake()
    {
        instance = this;
    }

    public Camera camera;

    public Game game;

    public Player player;

    public GameObject smallExplosionPrefab;
    public GameObject explosionPrefab;

    public Text scoreText;
    public Text infoText;

    public int score { get; set; }

    int nextScore = 200;

    int ellapsedTime;

    float enemySpawn;
    float itemSpawnTime;

    float enemySpawnTime;

    bool dead;

    public bool start { get; set; }
    public bool boss { get; set; }
    public bool spawn { get; set; }
    public bool autoShoot { get; set; }

    void Start()
    {
        Main.mode = Mode.Casual;

        Screen.orientation = ScreenOrientation.PortraitUpsideDown;

        start = false;
        boss = false;
        spawn = true;
        autoShoot = true;

        // init ship

        ShipBehaviour ship = AllShips.Ships[Game.ship1];

        player.GetComponent<SpriteRenderer>().sprite = ship.sprites[2];
        player.sprites = ship.sprites;
        player.health = ship.health;
        player.maxHealth = ship.health;
        player.shield = ship.shield;
        player.maxShield = ship.shield;
        player.shieldReloadDelay = ship.shieldReloadDelay;
        player.shieldReloadSpeed = ship.shieldReloadSpeed;
        player.shootDelay = new int[] { 30, 30, 30 };

        player.Init();

        ItemEvents.player = player;
        ItemEvents.i = 0;

        if (ship.buff1 != null && ship.buff1.Length != 0 && Game.buffs1 != null)
            ship.buff1[Game.buffs1[0]].trigger.Invoke();
        if (ship.buff2 != null && ship.buff2.Length != 0 && Game.buffs1 != null)
            ship.buff2[Game.buffs1[1]].trigger.Invoke();
        if (ship.buff3 != null && ship.buff3.Length != 0 && Game.buffs1 != null)
            ship.buff3[Game.buffs1[2]].trigger.Invoke();
    }

    void Update()
    {
        if (!start)
        {
            infoText.text = "Press to start";

            if (Input.anyKeyDown)
            {
                start = true;
                infoText.text = "";
            }

            return;
        }

        // move

        Move();

        // shoot

        if (!dead && autoShoot)
            player.Shoot(0, 1);

        scoreText.text = "Score: " + score * 10;

        // spawn items

        if (!boss && spawn)
            game.SpawnItems();

        // spawning

        ellapsedTime++;

        enemySpawn = Mathf.Abs(Mathf.Cos(ellapsedTime * 0.001f));

        int len = GameObject.FindGameObjectsWithTag("Enemy").Length;

        enemySpawnTime++;

        if (((enemySpawnTime > 20 && ((int)Random.Range(0, enemySpawn * 300) == 0 && len < 15)) || len == 0) && !boss && spawn)
        {
            while (true)
            {
                EnemyBehaviour enemy = AllEnemySprites.Enemybehaviour[Random.Range(0, AllEnemySprites.Enemybehaviour.Length)];

                if (!enemy.canSpawn || enemy.isBoss || score < enemy.requiredScore)
                    continue;

                if (Random.Range(0, enemy.probability) != 0)
                    continue;

                enemy.Summon();
                break;
            }

            enemySpawnTime = 0;
        }

        if (score >= nextScore)
        {
            spawn = false;
            boss = true;
        }

        if (boss)
        {
            if (len == 0)
            {
                nextScore = (int)(nextScore * 2.4f);

                Debug.Log(len);
                Debug.Log("spawn");

                // spawn boss
                while (true)
                {
                    EnemyBehaviour enemy = AllEnemySprites.Enemybehaviour[Random.Range(0, AllEnemySprites.Enemybehaviour.Length)];

                    if (!enemy.isBoss)
                        continue;

                    enemy.Summon();
                    break;
                }
            }
        }
    }

    void Move()
    {
        float deltaMove;

        deltaMove = camera.ScreenToWorldPoint(Input.mousePosition).x - player.transform.position.x;

        if (deltaMove > 1)
            deltaMove = 1;

        if (deltaMove < -1)
            deltaMove = -1;

        player.Move(deltaMove);
    }

    public void Die()
    {
        if (dead)
            return;

        infoText.text = "You died.\nscore: " + score * 10 + "\nhighscore: xxx";

        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = player.transform.position;

        player.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);

        dead = true;
    }
}

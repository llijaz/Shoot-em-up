using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

/*
 * TODO:
 * 
 * General:
 * √ Ship Chooser Menu
 * √ Main Menu
 * √ Online Game
 * - Campain
 * √ Bots
 * ? Bluetooth
 * - Connect with gitHub
 * - Updatecheck using gitHub
 * 
 * Fixes:
 * - Players go out of the screen
 * √ Remove double behaviours
 * √ Casual enemy out of screen softlock
 * √ Performance problems with collision detection and removing of bullets
 * - Remove useless bullets
 * - Bullet don't destroy hisself as client
 * - Crash when a item slowly disapeers is the Casual mode
 * - Items blocking bullets
 * 
 * Improvments:
 * √ Smooth movement
 * 
 * UI:
 * √ Multiplatform buttons with same sizes
 * √ UI don't block game
 * √ UI Press animations
 * √ Menu Button
 *  - Menu overlay
 * - New Menu
 * - Better online services
 * 
 * Graphics:
 * - Better winning screen
 * √ Dynamic ship sprite efect by movement
 * √ Health bars
 * - Boss health bar
 * - Background
 * - Planets
 * 
 * Bullets:
 * √ Custom Bullets
 * √ Vel to left and right
 * √ Multiple bullets
 * √ Items
 * √ Tripple-, Quad shot
 * √ Resize
 * x Deadshot
 * √ Laser
 * √ Spinning bullet
 * - Bullet follow enemy
 * - Diffrent types of bullets
 * 
 * Sound:
 * √ Music
 * √ Sound
 * 
 * Gameplay:
 * √ Movement
 * √ Shooting
 * √ Damage
 * √ Dying / Winning
 * √ Bullets
 * √ Items
 * √ Multiple bullets
 * - Drones
 * - Interactive items
 * - Asteroids
*/

/*
    countdown = a
    gametick = b
    shoots = c
    shoote = d
    item = e
    hit = f
    addItem = g
    shootr = h
    shootn = i
    requestItem = j
    move = k
    pos = l
    init = m 
    pos2 = n
    win = o
*/

public class Game : MonoBehaviour
{
    public static Game instance;

    public static int ship1;
    public static int ship2;

    public static int[] buffs1;
    public static int[] buffs2;

    public static GameObject BulletPrefab
    {
        get
        {
            return instance.bulletPrefab;
        }
    }

    private void Awake()
    {
        instance = this;

        // networking error
        // TODO: find a way to syncrosize diffrent framerates
        // e.g.: only update the movement of the player a fixed amount of times (could cause some lags at the player with higher framerate)

        Application.targetFrameRate = 60;
    }

    public bool onlyInitMode;

    public OnlineGame network;

    public Player player1;
    public Player player2;

    public GameObject bulletPrefab;
    public GameObject itemPrefab;

    public GameObject smallExplosionPrefab;
    public GameObject explosionPrefab;
    public GameObject hugeExplosionPrefab;

    public Canvas canvas;
    public Camera camera;

    public TextMeshPro infotext;

    public int touchDeathZone = 10;

    public int minItemSpawnTime = 300;
    public int maxItemSpawnTime = 1000;

    public bool shootControllsEnable1 { get; set; }
    public bool shootControllsEnable2 { get; set; }

    public float countDown;

    int currentItemSpawnTime = 0;
    int itemSpawnTime = 0;

    public float restartTime { get; set; }

    public bool gameTicks { get; set; }

    float updateTick;

    public int requestShoot { get; set; }

    void Start()
    {
        if (onlyInitMode)
            return;

        // setting player stats

        if (Main.mode == Mode.FreeGame && Main.bot)
        {
            gameObject.AddComponent<BattleBot>();

            ship2 = Random.Range(0, AllShips.Ships.Length);
            buffs2 = new int[]
            {
                Random.Range(0, AllShips.Ships[ship2].buff1.Length),
                Random.Range(0, AllShips.Ships[ship2].buff2.Length),
                Random.Range(0, AllShips.Ships[ship2].buff3.Length)
            };
        }

        InitPlayer(0);

        // init

        restartTime = 0;

        shootControllsEnable1 = true;
        shootControllsEnable2 = true;

        // networking

        if (Main.mode == Mode.FreeGame)
            gameTicks = true;

        if (Main.mode == Mode.Host)
        {
            infotext.text = "Wait for Client";
            GameObject.Find("Player2Canvas").active = false;
        }

        if (Main.mode == Mode.Join)
        {
            infotext.text = "Waiting for Host";
            GameObject.Find("Player2Canvas").active = false;
        }
    }

    void Update()
    {
        if (onlyInitMode)
            return;

        if (gameTicks)
        {
            ControllKeyboard();

            ControllTouch();

            if (Main.mode == Mode.Host)
            {
                SpawnItems();
            }

            if (Main.mode == Mode.FreeGame)
            {
                // restart after 400 frames

                if (restartTime != 0)
                {
                    restartTime += Time.deltaTime * 10;

                    if (restartTime > 100)
                    {
                        restartTime = 0;

                        // restart

                        Application.LoadLevel(1);
                    }
                }

                // spawn items

                SpawnItems();
            }

            if (Main.mode == Mode.Host)
            {
                updateTick += Time.deltaTime;

                network.Write("l " + -player1.transform.position.x);
                network.Write("n " + player2.transform.position.x);
                updateTick = 0;
            }
        }

        CountDown();
    }

    public void Win(string player)
    {
        if (Main.mode == Mode.Join)
            return;

        if (Main.mode == Mode.Host)
            network.Write("o " + player);

        infotext.text = "Player " + player + " won";

        restartTime = 1;
    }

    public void SetCountDown(int i)
    {
        countDown = i;
    }

    void ControllKeyboard()
    {
        // Move player 1

        if (Input.GetButton("Left"))
        {
            player1.Move(-0.5f);
        }

        if (Input.GetButton("Right"))
        {
            player1.Move(0.5f);
        }

        if (Input.GetButton("Fire"))
            RequestShoot(0);

        if (Input.GetButtonUp("Fire"))
            StopRequestShoot(0);

        // Move player 2

        if (shootControllsEnable2 && (Main.mode != Mode.Host || Main.mode != Mode.Join))
        {
            if (Input.GetButton("Left2"))
            {
                player2.Move(-0.5f);
            }

            if (Input.GetButton("Right2"))
            {
                player2.Move(0.5f);
            }

            if (Input.GetButton("Fire2"))
            {
                player2.Shoot(0, -1);
            }
        }
    }

    void ControllTouch()
    {
        float player1x = CrossPlatformInputManager.GetAxis("Player1x");
        float player2x = CrossPlatformInputManager.GetAxis("Player2x");

        player1.Move(player1x);
        player2.Move(player2x);

        if (shootControllsEnable1)
        {
            if (CrossPlatformInputManager.GetButton("Player1shoot1"))
                RequestShoot(0);

            if (CrossPlatformInputManager.GetButton("Player1shoot2"))
                RequestShoot(1);

            if (CrossPlatformInputManager.GetButton("Player1shoot3"))
                RequestShoot(2);

            if (CrossPlatformInputManager.GetButtonUp("Player1shoot1"))
                StopRequestShoot(0);

            if (CrossPlatformInputManager.GetButtonUp("Player1shoot2"))
                StopRequestShoot(1);

            if (CrossPlatformInputManager.GetButtonUp("Player1shoot3"))
                StopRequestShoot(2);
        }

        if (Main.mode == Mode.Host && requestShoot != -1)
        {
            player2.Shoot(requestShoot);
        }

        if (shootControllsEnable2 && (Main.mode != Mode.Host || Main.mode != Mode.Join))
        {
            if (CrossPlatformInputManager.GetButton("Player2shoot1"))
                player2.Shoot(0, -1);

            if (CrossPlatformInputManager.GetButton("Player2shoot2"))
                player2.Shoot(1, -1);

            if (CrossPlatformInputManager.GetButton("Player2shoot3"))
                player2.Shoot(2, -1);
        }
    }

    public void InitPlayer(int i)
    {
        if (i != 2)
        {
            ShipBehaviour ship = AllShips.Ships[ship1];

            player1.GetComponent<SpriteRenderer>().sprite = ship.sprites[2];
            player1.sprites = ship.sprites;
            player1.health = ship.health;
            player1.maxHealth = ship.health;
            player1.shield = ship.shield;
            player1.maxShield = ship.shield;
            player1.shieldReloadDelay = ship.shieldReloadDelay;
            player1.shieldReloadSpeed = ship.shieldReloadSpeed;
            player1.buttonCount = ship.buttonCount;
            player1.Init();

            if (ship.buff1 != null && ship.buff1.Length != 0 && buffs1 != null)
            {
                ItemEvents.player = player1;
                ItemEvents.i = 0;
                ship.buff1[buffs1[0]].trigger.Invoke();
            }
            if (ship.buff2 != null && ship.buff2.Length != 0 && buffs1 != null)
            {
                ItemEvents.player = player1;
                ItemEvents.i = 1;
                ship.buff2[buffs1[1]].trigger.Invoke();
            }
            if (ship.buff3 != null && ship.buff3.Length != 0 && buffs1 != null)
            {
                ItemEvents.player = player1;
                ItemEvents.i = 2;
                ship.buff3[buffs1[2]].trigger.Invoke();
            }
        }

        if (i != 1)
        {
            ShipBehaviour ship = AllShips.Ships[ship2];

            player2.GetComponent<SpriteRenderer>().sprite = ship.sprites[2];
            player2.sprites = ship.sprites;
            player2.health = ship.health;
            player2.maxHealth = ship.health;
            player2.shield = ship.shield;
            player2.maxShield = ship.shield;
            player2.shieldReloadDelay = ship.shieldReloadDelay;
            player2.shieldReloadSpeed = ship.shieldReloadSpeed;
            player2.buttonCount = ship.buttonCount;
            player2.Init();

            if (ship.buff1 != null && ship.buff1.Length != 0 && buffs2 != null)
            {
                ItemEvents.player = player2;
                ItemEvents.i = 0;
                ship.buff1[buffs2[0]].trigger.Invoke();
            }
            if (ship.buff2 != null && ship.buff2.Length != 0 && buffs2 != null)
            {
                ItemEvents.player = player2;
                ItemEvents.i = 1;
                ship.buff2[buffs2[1]].trigger.Invoke();
            }
            if (ship.buff3 != null && ship.buff3.Length != 0 && buffs2 != null)
            {
                ItemEvents.player = player2;
                ItemEvents.i = 2;
                ship.buff3[buffs2[2]].trigger.Invoke();
            }
        }
    }

    void CountDown()
    {
        if (countDown == -1)
            return;

        if (Main.mode == Mode.Host || Main.mode == Mode.Join)
        {
            network.Write("m " + ship1 + ":" + buffs1[0] + ":" + buffs1[1] + ":" + buffs1[2]);
        }

        if (countDown <= 0)
        {
            if (infotext.text == "GO")
            {
                infotext.text = "";
            }

            if (Main.mode == Mode.Host)
            {
                gameTicks = true;
                network.Write("b");
            }

            countDown = -1;

            return;
        }

        if ((int)(countDown + 0.5f) == 0)
        {
            infotext.text = "GO";
        }
        else
        {
            infotext.text = "Game starts in " + (int)(countDown + 0.5f);
        }

        countDown -= Time.deltaTime * 100 / 120;
    }

    void RequestShoot(int i)
    {
        if (Main.mode == Mode.Join)
        {
            if (requestShoot == -1)
                Game.instance.network.Write("h " + i);
            requestShoot = i;
            return;
        }

        player1.Shoot(i, 1);
    }

    void StopRequestShoot(int i)
    {
        Game.instance.network.Write("i " + i);
        requestShoot = -1;
    }

    public void SpawnItems()
    {
        if (currentItemSpawnTime == 0)
        {
            currentItemSpawnTime = Random.Range(minItemSpawnTime, maxItemSpawnTime);
        }

        itemSpawnTime++;

        if (itemSpawnTime > currentItemSpawnTime)
        {
            currentItemSpawnTime = 0;
            itemSpawnTime = 0;

            GameObject item = Instantiate(itemPrefab);
            item.GetComponent<Item>().Init();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class OnlineGame : Networking
{
    public override void Init()
    {
        if (Main.mode == Mode.Host)
        {
            SetIp(Main.ip);
            SetPort(Main.port);

            StartServer();
        }

        if (Main.mode == Mode.Join)
        {
            SetIp(Main.ip);
            SetPort(Main.port);

            StartClient();
        }
    }

    public override void Draw()
    {
        if (CrossPlatformInputManager.GetButton("StopNetwork"))
        {
            Stop();
        }
    }

    public override void GetMessage(string message)
    {
        Debug.Log("message recived: " + message);

        // MODE JOIN

        try
        {

            if (Main.mode == Mode.Join)
            {
                if (MessageEquals(message, "a"))
                {
                    Game.instance.countDown = 5;
                }

                if (MessageEquals(message, "b"))
                {
                    Game.instance.gameTicks = true;
                }

                if (MessageEquals(message.Split(' ')[0], "c"))
                {
                    int i = int.Parse(message.Split(' ')[1]);

                    Game.instance.player1.Shoot(i, true);
                }

                if (MessageEquals(message.Split(' ')[0], "d"))
                {
                    int i = int.Parse(message.Split(' ')[1]);

                    Game.instance.player2.Shoot(i, true);
                }

                if (MessageEquals(message.Split(' ')[0], "e"))
                {
                    string[] args = message.Split(' ')[1].Split(':');

                    int id = int.Parse(args[0]);
                    float x = float.Parse(args[1]);
                    float y = float.Parse(args[2]);
                    int dir = int.Parse(args[3]);

                    GameObject item = Instantiate(Game.instance.itemPrefab);
                    item.GetComponent<Item>().Init(id, x, y, dir);
                }

                if (MessageEquals(message.Split(' ')[0], "f"))
                {
                    string[] args = message.Split(' ')[1].Split(':');

                    string target = args[0];
                    int damage = int.Parse(args[1]);
                    int id = int.Parse(args[2]);

                    if (target.Equals("Player1"))
                    {
                        Game.instance.player2.Damage(damage);
                    }
                    else if (target.Equals("Player2"))
                    {
                        Game.instance.player1.Damage(damage);
                    }
                    else if (target.StartsWith("Item"))
                    {
                        // switch player id's

                        if (id == 1)
                            id = 2;
                        else if (id == 2)
                            id = 1;

                        GameObject.Find(target).GetComponent<Item>().Hit(damage, id);
                    }
                }

                if (MessageEquals(message.Split(' ')[0], "g"))
                {
                    string[] args = message.Split(' ')[1].Split(':');

                    int id = int.Parse(args[0]);
                    int i = int.Parse(args[1]);

                    if (id == 1)
                        id = 2;
                    else if (id == 2)
                        id = 1;

                    if (id == 1)
                    {
                        Game.instance.player1.itemButton.GetComponent<ItemSelector>().Trigger(i);
                    }
                    else
                    {
                        Game.instance.player2.itemButton.GetComponent<ItemSelector>().Trigger(i);
                    }
                }

                if (MessageEquals(message.Split(' ')[0], "l"))
                {
                    float i = float.Parse(message.Split(' ')[1]);
                    // Game.instance.player2.targetX = i;
                    Game.instance.player2.transform.position = new Vector2(i, Game.instance.player2.transform.position.y);
                }

                if (MessageEquals(message.Split(' ')[0], "n"))
                {
                    float i = float.Parse(message.Split(' ')[1]);
                    // Game.instance.player1.targetX = -i;
                    Game.instance.player1.transform.position = new Vector2(-i, Game.instance.player1.transform.position.y);
                }

                if (MessageEquals(message.Split(' ')[0], "o"))
                {
                    string i = message.Split(' ')[1];
                    // Game.instance.player1.targetX = -i;
                    Game.instance.infotext.text = "Player " + i + " won";
                    Game.instance.restartTime = 1;
                }
            }

            if (Main.mode == Mode.Host)
            {
                if (MessageEquals(message.Split(' ')[0], "h"))
                {
                    int i = int.Parse(message.Split(' ')[1]);

                    Game.instance.requestShoot = i;
                }

                if (MessageEquals(message.Split(' ')[0], "i"))
                {
                    Game.instance.requestShoot = -1;
                }

                if (MessageEquals(message.Split(' ')[0], "j"))
                {
                    int i = int.Parse(message.Split(' ')[1]);

                    Game.instance.player2.itemButton.GetComponent<ItemSelector>().Trigger(i);
                }
            }

            if (MessageEquals(message.Split(' ')[0], "k"))
            {
                float i = float.Parse(message.Split(' ')[1]);

                if (i > 1)
                    i = 1;

                if (i < -1)
                    i = -1;

                Game.instance.player2.Move(i);
            }

            if (MessageEquals(message.Split(' ')[0], "m"))
            {
                string[] args = message.Split(' ')[1].Split(':');

                Game.ship2 = int.Parse(args[0]);
                Game.buffs2 = new int[] {
                    int.Parse(args[1]), int.Parse(args[2]), int.Parse(args[3]),
                };

                Game.instance.InitPlayer(2);
            }

        }
        catch { }
    }
}

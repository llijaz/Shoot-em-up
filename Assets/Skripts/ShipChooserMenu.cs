using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class ShipChooserMenu : MonoBehaviour
{
    public GameObject canv2;

    public Image player1sprite;
    public Image player2sprite;

    public Image[] player1buffs;
    public Image[] player2buffs;

    public Text player1text;
    public Text player2text;

    int player1ship;
    int player2ship;

    bool player1ready;
    bool player2ready;

    int[] player1buffsindex;
    int[] player2buffsindex;

    void Start()
    {
        player1buffsindex = new int[] { 0, 0, 0 };
        player2buffsindex = new int[] { 0, 0, 0 };

        UpdateShips();

        if (Main.mode == Mode.FreeGame && Main.bot == false)
        {
            canv2.active = true;
        }
        else
        {
            canv2.active = false;
        }
    }

    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Player1Left"))
        {
            player1ship--;
            if (player1ship < 0)
            {
                player1ship = AllShips.Ships.Length - 1;
            }

            UpdateShips(1);
        }

        if (CrossPlatformInputManager.GetButtonDown("Player2Left"))
        {
            player2ship--;
            if (player2ship < 0)
            {
                player2ship = AllShips.Ships.Length - 1;
            }

            UpdateShips(2);
        }

        if (CrossPlatformInputManager.GetButtonDown("Player1Right"))
        {
            player1ship++;
            if (player1ship > AllShips.Ships.Length - 1)
            {
                player1ship = 0;
            }

            UpdateShips(1);
        }

        if (CrossPlatformInputManager.GetButtonDown("Player2Right"))
        {
            player2ship++;
            if (player2ship > AllShips.Ships.Length - 1)
            {
                player2ship = 0;
            }

            UpdateShips(2);
        }

        if (CrossPlatformInputManager.GetButtonDown("Player1Ready"))
            player1ready = true;

        if (CrossPlatformInputManager.GetButtonDown("Player2Ready"))
            player2ready = true;

        if (CrossPlatformInputManager.GetButtonDown("Player1Buff1"))
        {
            player1buffsindex[0]++;
            if (player1buffsindex[0] >= AllShips.Ships[player1ship].buff1.Length)
                player1buffsindex[0] = 0;
            UpdateBuffs();
        }

        if (CrossPlatformInputManager.GetButtonDown("Player1Buff2"))
        {
            player1buffsindex[1]++;
            if (player1buffsindex[1] >= AllShips.Ships[player1ship].buff2.Length)
                player1buffsindex[1] = 0;
            UpdateBuffs();
        }

        if (CrossPlatformInputManager.GetButtonDown("Player1Buff3"))
        {
            player1buffsindex[2]++;
            if (player1buffsindex[2] >= AllShips.Ships[player1ship].buff3.Length)
                player1buffsindex[2] = 0;
            UpdateBuffs();
        }

        if (CrossPlatformInputManager.GetButtonDown("Player2Buff1"))
        {
            player2buffsindex[0]++;
            if (player2buffsindex[0] >= AllShips.Ships[player2ship].buff1.Length)
                player2buffsindex[0] = 0;
            UpdateBuffs();
        }

        if (CrossPlatformInputManager.GetButtonDown("Player2Buff2"))
        {
            player2buffsindex[1]++;
            if (player2buffsindex[1] >= AllShips.Ships[player2ship].buff2.Length)
                player2buffsindex[1] = 0;
            UpdateBuffs();
        }

        if (CrossPlatformInputManager.GetButtonDown("Player2Buff3"))
        {
            player2buffsindex[2]++;
            if (player2buffsindex[2] >= AllShips.Ships[player2ship].buff3.Length)
                player2buffsindex[2] = 0;
            UpdateBuffs();
        }

        if (player1ready && (player2ready || Main.mode != Mode.FreeGame || (Main.mode == Mode.FreeGame && Main.bot)))
        {
            Game.ship1 = player1ship;
            Game.ship2 = player2ship;

            Game.buffs1 = player1buffsindex;
            Game.buffs2 = player2buffsindex;

            if (Main.mode == Mode.Casual)
            {
                Application.LoadLevel(3);
            } else
            {
                Application.LoadLevel(2);
            }
        }
    }

    void UpdateShips()
    {
        UpdateShips(1);
        UpdateShips(2);
    }

    void UpdateShips(int s)
    {
        if (s == 1)
        {
            ShipBehaviour ship1 = AllShips.Ships[player1ship];
            player1sprite.sprite = ship1.sprites[2];
            player1text.text = ship1.name;
            player1buffsindex = new int[] { 0, 0, 0 };
        }

        if (s == 2)
        {
            ShipBehaviour ship2 = AllShips.Ships[player2ship];
            player2sprite.sprite = ship2.sprites[2];
            player2text.text = ship2.name;
            player2buffsindex = new int[] { 0, 0, 0 };
        }

        UpdateBuffs();
    }

    void UpdateBuffs()
    {
        ShipBehaviour ship1 = AllShips.Ships[player1ship];
        ShipBehaviour ship2 = AllShips.Ships[player2ship];

        player1buffs[2].gameObject.active = true;
        player1buffs[1].gameObject.active = true;
        player2buffs[2].gameObject.active = true;
        player2buffs[1].gameObject.active = true;

        player1buffs[0].sprite = ship1.buff1[player1buffsindex[0]].sprite;
        player2buffs[0].sprite = ship2.buff1[player2buffsindex[0]].sprite;

        if (ship1.buff3.Length == 0)
            player1buffs[2].gameObject.active = false;
        else
            player1buffs[2].sprite = ship1.buff3[player1buffsindex[2]].sprite;

        if (ship1.buff2.Length == 0)
            player1buffs[1].gameObject.active = false;
        else
            player1buffs[1].sprite = ship1.buff2[player1buffsindex[1]].sprite;

        if (ship2.buff3.Length == 0)
            player2buffs[2].gameObject.active = false;
        else
            player2buffs[2].sprite = ship2.buff3[player2buffsindex[2]].sprite;

        if (ship2.buff2.Length == 0)
            player2buffs[1].gameObject.active = false;
        else
            player2buffs[1].sprite = ship2.buff2[player2buffsindex[1]].sprite;
    }
}

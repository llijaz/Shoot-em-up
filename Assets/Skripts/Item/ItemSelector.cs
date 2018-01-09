using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class ItemSelector : MonoBehaviour
{
    public float speed = 0.01f;

    public Player player;

    public ItemBehaviour item;

    Vector2 startPosition;
    Sprite startSprite;

    void Start()
    {
        startPosition = transform.position;
        startSprite = GetComponent<Image>().sprite;
    }

    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, startPosition, speed);

        if (CrossPlatformInputManager.GetButtonDown("Player" + player.GetPlayerID() + "Item"))
        {
            if (item != null)
            {
                if (player.GetPlayerID() == 1)
                    Game.instance.shootControllsEnable1 = !Game.instance.shootControllsEnable1;
                else
                    Game.instance.shootControllsEnable2 = !Game.instance.shootControllsEnable2;
            }
        }

        if ((!Game.instance.shootControllsEnable1 && player.GetPlayerID() == 1) || (!Game.instance.shootControllsEnable2 && player.GetPlayerID() == 2))
        {
            if (CrossPlatformInputManager.GetButton("Player" + player.GetPlayerID() + "shoot1"))
            {
                if (Main.mode == Mode.Join)
                {
                    Game.instance.network.Write("j 0");
                    return;
                }

                Trigger(0);
            }

            if (CrossPlatformInputManager.GetButton("Player" + player.GetPlayerID() + "shoot2"))
            {
                if (Main.mode == Mode.Join)
                {
                    Game.instance.network.Write("j 1");
                    return;
                }

                Trigger(1);
            }

            if (CrossPlatformInputManager.GetButton("Player" + player.GetPlayerID() + "shoot3"))
            {
                if (Main.mode == Mode.Join)
                {
                    Game.instance.network.Write("j 2");
                    return;
                }

                Trigger(2);
            }
        }
    }

    public void Trigger(int i)
    {
        if (item == null)
            return;

        if (player.GetPlayerID() == 1)
            Game.instance.shootControllsEnable1 = true;
        else
            Game.instance.shootControllsEnable2 = true;

        if (Main.mode == Mode.Host)
        {
            Game.instance.network.Write("g " + player.GetPlayerID() + ":" + i);
        }

        ItemEvents.player = player;
        ItemEvents.i = i;

        item.trigger.Invoke();

        GetComponent<Image>().sprite = startSprite;
        item = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBot : MonoBehaviour
{
    int ellapsedTime = 0;
    int movementTime = 0;

    int goInDir = 0;

    int shootIndex = 0;

    int dir = 0;

    void Start()
    {
        ellapsedTime = 0;
        goInDir = 0;

        dir = Random.Range(0, 1) * 2 - 1;
    }

    void Update()
    {
        ellapsedTime++;

        // move

        Move();

        // shoot

        Shoot();
    }

    void Move()
    {
        movementTime = Random.Range(1, 10);

        if (goInDir == 0)
        {
            Game.instance.player2.Move(Mathf.Abs(Mathf.Sin(movementTime * 0.1f)) * dir);

            if (Game.instance.player2.transform.position.x < -4)
            {
                goInDir = Random.Range(-50, -10);
            }

            if (Game.instance.player2.transform.position.x > 4)
            {
                goInDir = Random.Range(10, 50);
            }
        }
        else
        {
            if (Random.Range(0, 4) != 0)
                dir = (Game.instance.player1.transform.position.x > Game.instance.player2.transform.position.x ? 1 : -1);

            if (GameObject.FindGameObjectWithTag("Item"))
            {
                GameObject go = GameObject.FindGameObjectWithTag("Item");
                dir = (go.transform.position.x > Game.instance.player2.transform.position.x ? 1 : -1);
            }

            if (goInDir > 0)
            {
                Game.instance.player2.Move(-1);
                goInDir--;
            }
            else
            {
                Game.instance.player2.Move(1);
                goInDir++;
            }
        }
    }

    void Shoot()
    {
        if (Random.Range(0, 20) == 0)
        {
            shootIndex = Random.Range(0, Game.instance.player2.buttonCount);
        }

        if (Random.Range(0, (int)(Mathf.Cos(ellapsedTime * 0.01f) * 20)) == 0)
        {
            Game.instance.player2.Shoot(shootIndex, -1);
        }

        if (Game.instance.player2.itemButton.GetComponent<ItemSelector>().item != null)
        {
            Game.instance.player2.itemButton.GetComponent<ItemSelector>().Trigger(shootIndex);
        }
    }
}

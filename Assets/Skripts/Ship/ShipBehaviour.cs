using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Ship", menuName = "Custom/Ship")]
public class ShipBehaviour : ScriptableObject
{
    new public string name = "Unnamed ship";

    public int health = 100;
    public int shield = 100;

    public int shieldReloadDelay = 50;
    public float shieldReloadSpeed = 0.5f;

    public float speed = 0.07f;

    public int buttonCount = 2;

    public Sprite[] sprites;

    public ItemBehaviour[] buff1;
    public ItemBehaviour[] buff2;
    public ItemBehaviour[] buff3;
}

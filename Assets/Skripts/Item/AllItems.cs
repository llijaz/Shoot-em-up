using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllItems : MonoBehaviour
{
    public static List<ItemBehaviour> Items;

    public List<ItemBehaviour> items;

    private void Awake()
    {
        Items = items;
    }
}

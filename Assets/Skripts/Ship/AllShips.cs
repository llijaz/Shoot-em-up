using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllShips : MonoBehaviour
{
    public static ShipBehaviour[] Ships;

    public ShipBehaviour[] ships;

    private void Awake()
    {
        Ships = ships;
    }
}

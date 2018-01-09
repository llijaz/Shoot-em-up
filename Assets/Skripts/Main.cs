using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode
{
    FreeGame, Host, Join, Casual
}

public class Main
{
    public static Mode mode;

    public static string ip;
    public static int port;

    public static bool bot;
}

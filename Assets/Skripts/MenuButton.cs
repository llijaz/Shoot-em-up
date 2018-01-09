using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MenuButton : MonoBehaviour
{
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("menu"))
        {
            Application.LoadLevel(0);
        }
    }
}

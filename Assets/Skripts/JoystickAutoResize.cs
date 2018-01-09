using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class JoystickAutoResize : MonoBehaviour
{
    /*
     * Not resizeable yet!
    */

    public CanvasScaler scaler;

    Joystick joystick;

    int startMovementRange;

    void Start()
    {
        joystick = GetComponent<Joystick>();

        startMovementRange = joystick.MovementRange;

        Resize();
    }

    void Resize()
    {
        joystick.MovementRange = (int) (startMovementRange * scaler.transform.localScale.x);
    }

}

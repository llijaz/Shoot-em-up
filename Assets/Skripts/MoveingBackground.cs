using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveingBackground : MonoBehaviour
{
    public Camera camera;

    public float colorLerp;

    Color targetColor;
    float colorTime = 1;

    void Start()
    {
        if (camera == null)
        {
            camera = Camera.current;
        }
    }

    void Update()
    {
        ChangeBackgroundColor();
    }

    void ChangeBackgroundColor()
    {
        colorTime += colorLerp;

        if (colorTime > 0.9f)
        {
            targetColor = Random.ColorHSV();
            targetColor = new Color(targetColor.r * 0.3f, targetColor.g * 0.3f, targetColor.b * 0.3f);

            colorTime = 0;
        }

        camera.backgroundColor = Color.Lerp(camera.backgroundColor, targetColor, colorLerp);
    }
}

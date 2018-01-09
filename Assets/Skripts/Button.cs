using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Button : MonoBehaviour
{
    public string pressTrigger;

    public float speed = 0.1f; 

    public Vector3 extraSize = new Vector3(1.25f, 1.25f, 1);

    Vector3 startSize;
    Vector3 targetSize;

    void Start()
    {
        startSize = transform.localScale;
        targetSize = transform.localScale;

        ButtonHandler handler = GetComponent<ButtonHandler>();

        if (handler != null)
        {
            pressTrigger = handler.Name;
        }
    }

    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown(pressTrigger))
        {
            targetSize = new Vector3(startSize.x * extraSize.x, startSize.y * extraSize.y, startSize.z * extraSize.z);
        }

        if (CrossPlatformInputManager.GetButtonUp(pressTrigger))
        {
            targetSize = startSize;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, targetSize, speed);
    }
}

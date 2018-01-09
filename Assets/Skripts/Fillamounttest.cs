using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fillamounttest : MonoBehaviour
{
    [Range(0, 1)]
    public float i = 0;

    public Canvas canvas;

    public Camera camera;

    public Transform target;

    public Vector3 offset;

    void Start()
    {
    }

    void Update()
    {
        RectTransform CanvasRect = canvas.GetComponent<RectTransform>();

        // transform.position = RectTransformUtility.WorldToScreenPoint(Camera.current, target.transform.position);
        transform.position = camera.WorldToViewportPoint(target.position) + offset;

        Vector2 WorldObject_ScreenPosition = new Vector2(
 ((transform.position.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
 ((transform.position.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        //now you can set the position of the ui element
        GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;

        GetComponent<Image>().fillAmount = i;
    }
}

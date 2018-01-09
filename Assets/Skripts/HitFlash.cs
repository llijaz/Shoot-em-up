using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    public static Material material;

    public Material _material;

    Material startMaterial;

    Color startColor;

    int flash;

    private void Awake()
    {
        if (_material != null)
            material = _material;
    }

    private void Start()
    {
        startMaterial = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        try
        {
            if (flash >= 1)
            {
                flash++;

                if (flash == 6)
                {
                    // GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                    GetComponent<Renderer>().material = startMaterial;
                }


                if (flash > 10)
                {
                    GetComponent<SpriteRenderer>().color = Color.white;
                    GetComponent<Renderer>().material = startMaterial;
                    flash = 0;
                }
            }
        }
        catch { }
    }

    public void Hit()
    {
        try
        {
            GetComponent<AudioSource>().Play();
        }
        catch { }
        try
        {
            GetComponent<Renderer>().material = material;

            flash = 1;
        }
        catch { }
    }
}

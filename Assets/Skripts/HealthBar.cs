using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface HealthBarChange
{
    void OnHealthChange();
}

public class HealthBar : MonoBehaviour, HealthBarChange
{
    public Player player;

    public Image healthImg;
    public Image shieldImg;

    public float speed = 0.025f;

    float newHealth;
    float newShield;

    void Start()
    {
        player.healthBarChange = this;

        newHealth = 1;
        newShield = 1;
    }

    void Update()
    {
        healthImg.fillAmount = Mathf.Lerp(healthImg.fillAmount, newHealth, speed);
        shieldImg.fillAmount = Mathf.Lerp(shieldImg.fillAmount, newShield, speed);
    }

    public void OnHealthChange()
    {
        newHealth = (float)player.health / player.maxHealth;
        newShield = (float)player.shield / player.maxShield;
    }
}

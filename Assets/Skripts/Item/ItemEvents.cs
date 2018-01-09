using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEvents : MonoBehaviour
{
    public static Player player;

    public static int i;
    
    public void Powerup()
    {
        player.AddBulletBehaviour(i, new BulletUp());
    }

    public void Element()
    {
        player.AddBulletBehaviour(i, new BulletElement());
    }

    public void Firerate()
    {
        for (int i = 0; i < 3; i++)
        {
            player.shootDelay[i] = (int)(player.shootDelay[i] * 0.7f);
        }
    }

    public void Resize()
    {
        player.AddBulletBehaviour(i, new BulletSize());
    }

    public void Ball()
    {
        player.AddBulletBehaviour(i, new BulletBall());
    }

    public void Wave()
    {
        player.AddBulletBehaviour(i, new BulletWave());
    }

    public void Missile()
    {
        player.AddBulletBehaviour(i, new BulletMissile());
    }

    public void DoubleShot()
    {
        player.AddBulletBehaviour(i, new BulletDoubleShot());
    }

    public void TribbleShot()
    {
        player.AddBulletBehaviour(i, new BulletTribbleShot());
    }

    public void Pump()
    {
        player.AddBulletBehaviour(i, new BulletPump());
    }

    public void SpeedUp()
    {
        player.speed *= 1.2f;
    }

    public void Grenate()
    {
        player.AddBulletBehaviour(i, new BulletGrenate());
    }

    public void Laser()
    {
        player.AddBulletBehaviour(i, new BulletLaser());
    }

    public void Twist()
    {
        player.AddBulletBehaviour(i, new BulletTwist());
    }

}

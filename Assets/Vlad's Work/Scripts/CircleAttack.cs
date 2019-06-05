using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttack : Attack
{
    public override void PerformAttack(List<GameObject> targets, float m_bulletRadius)
    {
        if (targets.Count == 0)
        {
            return;
        }

        float currRot = 0;

        for (int i = 0; i < 8; i++)
        {
            EntityFactory.Factory.CreateBullet(transform.position, AngleToVector(currRot), m_bulletRadius, m_bulletSpeed, m_bulletDamage, m_bulletColor);
            currRot += 45f;
        }
    }

    //returns a normalized vector assuming counter-clockwise direction,
    //with angle = 0 being equivalent to Vector.right
    private static Vector2 AngleToVector(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

}

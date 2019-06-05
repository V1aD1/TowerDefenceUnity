using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float m_bulletSpeed;
    public float m_bulletDamage;
    public Color m_bulletColor;

    public virtual void PerformAttack(List<GameObject> targets, float m_bulletRadius)
    {
        if (targets.Count == 0)
        {
            return;
        }

        EntityFactory.Factory.CreateBullet(transform.position, targets[0].transform.position - transform.position, m_bulletRadius, m_bulletSpeed, m_bulletDamage, m_bulletColor);
    }
}

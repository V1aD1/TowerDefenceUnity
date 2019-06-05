using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathDelegate : MonoBehaviour
{
    public delegate void EnemyDelegate(GameObject enemy);
    public EnemyDelegate m_enemyDelegate;

    private void OnDisable()
    {
        m_enemyDelegate?.Invoke(gameObject);
    }
}

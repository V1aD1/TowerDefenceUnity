using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReachedDestDelegate : MonoBehaviour
{
    public delegate void EnemyDelegate();
    public EnemyDelegate m_enemyDelegate;

    public void OnReachedDest()
    {
        m_enemyDelegate?.Invoke();
    }
}

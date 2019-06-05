using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float m_attackRadius = 0.7f;

    private float m_rateOfFire = 1f;
    private float m_timePassed;
    private List<GameObject> m_enemiesInRange;
    private Attack m_attackComponent;

    // Start is called before the first frame update
    private void Start()
    {
        m_timePassed = Time.deltaTime;
        m_enemiesInRange = new List<GameObject>();
        m_attackComponent = gameObject.GetComponent<Attack>();

        if (m_attackComponent == null)
        {
            Debug.Log("No Attack component found for this tower!");
        }

        var attackRange = gameObject.AddComponent<CircleCollider2D>();
        attackRange.radius = m_attackRadius;
    }

    void OnEnable()
    {
        //this line doesn't actually set the m_attackComponent, 
        //so I have to do a GetComponent<Attack>() in the Update method...
        m_attackComponent = gameObject.GetComponent<Attack>();
    }

    // Update is called once per frame
    void Update()
    {

        if (m_timePassed > m_rateOfFire)
        {
            m_attackComponent = gameObject.GetComponent<Attack>();

            //necessary to ensure bullet radius is equal to tower attack radius
            m_attackComponent.PerformAttack(m_enemiesInRange, m_attackRadius * Mathf.Max(transform.localScale.x, transform.localScale.y));
            m_timePassed = 0f;
        }

        m_timePassed += Time.deltaTime;
    }

    private void SubscribeToDelegates(GameObject gameObject)
    {
        EnemyDeathDelegate del = gameObject.GetComponent<EnemyDeathDelegate>();

        if (del == null)
        {
            Debug.LogError("Enemy doesn't have a death delegate to monitor!");
        }

        else
        {
            del.m_enemyDelegate += OnEnemyDeath;
        }
    }

    private void UnsubscribeFromDelegates(GameObject gameObject)
    {
        EnemyDeathDelegate del = gameObject.GetComponent<EnemyDeathDelegate>();

        if (del == null)
        {
            Debug.LogError("Enemy doesn't have a death delegate to monitor!");
        }

        else
        {
            del.m_enemyDelegate -= OnEnemyDeath;
        }
    }

    private void OnEnemyDeath(GameObject enemy)
    {
        m_enemiesInRange.Remove(enemy);
        UnsubscribeFromDelegates(enemy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            m_enemiesInRange.Add(collision.gameObject);
            SubscribeToDelegates(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            m_enemiesInRange.Remove(collision.gameObject);
            UnsubscribeFromDelegates(collision.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float m_startHealth = 1f;
    public float m_speed = 5f;

    private float m_health;
    private Vector3 m_velocity;
    private List<Vector3> m_destinations = new List<Vector3>();
    private int m_currDestIndex;
    private Vector3 m_currDest;
    private bool m_arrivedAtCurrDest = false;

    private SpriteRenderer m_spriteRenderer;



    // Start is called before the first frame update
    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        BeginJourney();
    }

    private void OnEnable()
    {
        m_health = m_startHealth;
        BeginJourney();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_health <= 0)
        {
            Die();
        }

        m_spriteRenderer.color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.g, m_spriteRenderer.color.b, m_health / m_startHealth);

        //ensuring enemy doesn't step over the destination
        if (((transform.position + m_velocity * Time.deltaTime) - transform.position).magnitude > (transform.position - m_currDest).magnitude)
        {
            transform.position = m_currDest;
            m_arrivedAtCurrDest = true;
        }

        else
        {
            transform.Translate(m_velocity * Time.deltaTime);
        }

        if (m_arrivedAtCurrDest)
        {
            m_currDestIndex++;

            if (m_currDestIndex < m_destinations.Count)
            {
                m_currDest = m_destinations[m_currDestIndex];
                ComputeNewVelocity();
            }

            //the enemy has reached it's final destination, it should disable itself
            else
            {
                //announce that destination reached
                GetComponent<EnemyReachedDestDelegate>()?.OnReachedDest();                
                Die();
            }

            m_arrivedAtCurrDest = false;
        }
    }

    public void Initialize(List<Vector3> newDestinations)
    {
        m_destinations = newDestinations;

        if (m_currDestIndex < m_destinations.Count)
        {
            m_currDest = m_destinations[m_currDestIndex];
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    private void BeginJourney()
    {
        m_currDestIndex = 0;
        if (m_currDestIndex < m_destinations.Count)
        {
            m_currDest = m_destinations[m_currDestIndex];
            ComputeNewVelocity();
        }
    }

    private void ComputeNewVelocity()
    {
        m_velocity = (m_currDest - transform.position).normalized * m_speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("Enemy collided with bullet!");
            var bullet = collision.gameObject.GetComponent<Bullet>();

            HandleBulletCollision(bullet);
        }
    }

    private void HandleBulletCollision(Bullet bullet)
    {
        m_health -= bullet.m_damage;
    }
}

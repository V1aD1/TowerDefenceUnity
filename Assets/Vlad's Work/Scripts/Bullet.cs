using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float m_radius = 0.7f;
    public float m_damage = 1f;
    public float m_speed = 5f;

    private Vector3 m_startPosition;
    private Vector2 m_direction = new Vector2(1, 1);
    private Vector2 m_velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        m_startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(m_velocity * Time.deltaTime);

        if((transform.position - m_startPosition).magnitude > m_radius)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet collided with something!");
        Die();
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    public void SetDirection(Vector2 newDir)
    {
        m_direction = newDir;
        m_direction.Normalize();

        //to ensure velocity has the speed specified
        m_velocity = m_direction * m_speed;
    }
}

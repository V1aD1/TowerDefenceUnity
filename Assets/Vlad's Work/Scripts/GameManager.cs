using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject m_gameOverPanel;
    public int m_currLives = 5;


    private float m_spawnRate = 3.0f;
    private float m_timeSinceLastEnemySpawn;
    private float m_chanceOfMediumEnemy = 50;
    private float m_chanceOfHardEnemy = 75;

    private float m_difficultyRate = 1;
    private float m_spawnRateReduction = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        m_timeSinceLastEnemySpawn += Time.deltaTime;

        if (m_currLives <= 0)
        {
            Time.timeScale = 0.00001f;
            m_gameOverPanel.gameObject.SetActive(true);
        }

        else
        {
            //difficulty increases as time goes by
            if (m_chanceOfMediumEnemy > 25)
            {
                m_chanceOfMediumEnemy -= m_difficultyRate * Time.deltaTime;
            }

            if (m_chanceOfHardEnemy > 50)
            {
                m_chanceOfHardEnemy -= m_difficultyRate * Time.deltaTime;
            }

            if (m_spawnRate > 0.5f)
            {
                m_spawnRate -= Time.deltaTime * m_spawnRateReduction;
            }

            //spawn enemies
            if (m_timeSinceLastEnemySpawn > m_spawnRate)
            {
                var rand = Random.Range(0, 100);

                if (rand < m_chanceOfMediumEnemy)
                {
                    SubscribeToDelegates(GridManager.Grid.SpawnEnemy(0));
                }

                else if (rand < m_chanceOfHardEnemy)
                {
                    SubscribeToDelegates(GridManager.Grid.SpawnEnemy(1));
                }

                else
                {
                    SubscribeToDelegates(GridManager.Grid.SpawnEnemy(2));
                }

                m_timeSinceLastEnemySpawn = 0.0f;
            }

            //handle user input
            if (Input.GetMouseButton(0))
            {
                PreviewObject.Preview.GenerateTowerPreview(0);
            }

            else if (Input.GetMouseButton(1))
            {
                PreviewObject.Preview.GenerateTowerPreview(1);
            }

            else
            {
                PreviewObject.Preview.gameObject.SetActive(false);
            }

            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                GridManager.Grid.SpawnTower(hit, 0);
            }

            if (Input.GetMouseButtonUp(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                GridManager.Grid.SpawnTower(hit, 1);
            }
        }
    }

    private void SubscribeToDelegates(GameObject gameObject)
    {
        EnemyDeathDelegate deathDel = gameObject?.GetComponent<EnemyDeathDelegate>();
        EnemyReachedDestDelegate destDel = gameObject?.GetComponent<EnemyReachedDestDelegate>();

        if(deathDel == null)
        {
            Debug.LogError("Enemy doesn't have a DEATH delegate to monitor!");
        }

        else
        {
            deathDel.m_enemyDelegate += EnemyDeath; 
        }

        if (destDel == null)
        {
            Debug.LogError("Enemy doesn't have a reached DEST delegate to monitor!");
        }

        else
        {
            destDel.m_enemyDelegate += EnemyReachedDest;
        }
    }

    private void UnsubscribeFromDelegates(GameObject gameObject)
    {
        EnemyDeathDelegate deathDel = gameObject?.GetComponent<EnemyDeathDelegate>();
        EnemyReachedDestDelegate destDel = gameObject?.GetComponent<EnemyReachedDestDelegate>();

        if (deathDel == null)
        {
            Debug.LogError("Enemy doesn't have a DEATH delegate to monitor!");
        }

        else
        {
            deathDel.m_enemyDelegate -= EnemyDeath;
        }

        if (destDel == null)
        {
            Debug.LogError("Enemy doesn't have a reached DEST delegate to monitor!");
        }

        else
        {
            destDel.m_enemyDelegate -= EnemyReachedDest;
        }
    }

    private void EnemyReachedDest()
    {
        Debug.Log("Enemy reached dest!");
        m_currLives -= 1;
    }

    private void EnemyDeath(GameObject enemy)
    {
        UnsubscribeFromDelegates(enemy);
    }
}

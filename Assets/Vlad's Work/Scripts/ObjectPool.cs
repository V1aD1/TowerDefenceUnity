using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject m_towerPrefab;
    public GameObject m_enemyPrefab;
    public GameObject m_bulletPrefab;

    public int m_bulletPoolSize = 500;
    public int m_enemyPoolSize = 100;
    public int m_towerPoolSize = 200;

    public static ObjectPool Pool;

    private List<GameObject> m_bulletPool;
    private List<GameObject> m_enemyPool;
    private List<GameObject> m_towerPool;
    private List<Vector3> m_pathCornersInWorldSpace = new List<Vector3>();

    private int m_poolResize = 10;

    void Awake()
    {
        Pool = this;
        m_bulletPool = new List<GameObject>();
        m_enemyPool = new List<GameObject>();
        m_towerPool = new List<GameObject>();

        //allocate object pools    
        IncreaseBulletPool(m_bulletPoolSize);
        IncreaseEnemyPool(m_enemyPoolSize);
        IncreaseTowerPool(m_towerPoolSize);
    }

    void Start()
    {
        //assuming the corners of the path won't change once the level has started
        foreach (var corner in GridManager.Grid.m_pathCorners)
        {
            m_pathCornersInWorldSpace.Add(GridManager.Grid.GetCentreOfTile(new Vector3Int(corner.x, corner.y, 0)));
        }

        InitializeEnemyPool();
    }

    public void InitializeEnemyPool()
    {
        foreach (var enemy in m_enemyPool)
        {
            enemy.GetComponent<Enemy>().Initialize(m_pathCornersInWorldSpace);
        }
    }

    public GameObject GetBullet()
    {
        for(int i = 0; i< m_bulletPool.Count; i++)
        {
            if (!m_bulletPool[i].activeInHierarchy)
            {
                return m_bulletPool[i];
            }
        }

        //need to resize the pool
        IncreaseBulletPool(m_poolResize);

        return GetBullet();
    }

    public GameObject GetTower()
    {
        for (int i = 0; i < m_towerPool.Count; i++)
        {
            if (!m_towerPool[i].activeInHierarchy)
            {
                return m_towerPool[i];
            }
        }

        //need to resize the pool
        IncreaseTowerPool(m_poolResize);

        return GetTower();
    }

    public GameObject GetEnemy()
    {
        for (int i = 0; i < m_enemyPool.Count; i++)
        {
            if (!m_enemyPool[i].activeInHierarchy)
            {
                return m_enemyPool[i];
            }
        }

        //need to resize the pool
        IncreaseEnemyPool(m_poolResize);

        return GetEnemy();
    }

    private void IncreaseBulletPool(int numToAdd)
    {
        for (int i = 0; i < numToAdd; i++)
        {
            GameObject bullet = Instantiate(m_bulletPrefab);
            bullet.SetActive(false);
            m_bulletPool.Add(bullet);
        }
    }

    private void IncreaseEnemyPool(int numToAdd)
    {
        for (int i = 0; i < numToAdd; i++)
        {
            GameObject enemy = Instantiate(m_enemyPrefab);
            enemy.GetComponent<Enemy>().Initialize(m_pathCornersInWorldSpace);
            enemy.SetActive(false);
            m_enemyPool.Add(enemy);
        }
    }

    private void IncreaseTowerPool(int numToAdd)
    {
        for (int i = 0; i < numToAdd; i++)
        {
            GameObject tower = Instantiate(m_towerPrefab);
            tower.SetActive(false);
            m_towerPool.Add(tower);
        }
    }

}

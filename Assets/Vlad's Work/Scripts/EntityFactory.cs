using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFactory : MonoBehaviour
{
    public float m_enemyWeakSpeed = 1f;
    public int m_enemyWeakStartHealth = 1;
    public Color m_enemyWeakColor = new Color(0f, 255f, 0f);

    public float m_enemyMediumSpeed = 2f;
    public int m_enemyMediumStartHealth = 5;
    public Color m_enemyMediumColor = new Color(255f, 255f, 0f);

    public float m_enemyStrongSpeed = 1f;
    public int m_enemyStrongStartHealth = 10;
    public Color m_enemyStrongColor = new Color(255f, 0f, 0f);

    public float m_directTowerRadius = 0.7f;
    public int m_directTowerBulletDamage = 1;
    public float m_directTowerBulletSpeed = 5f;
    public Color m_directTowerColor = new Color(255, 255, 255);

    public float m_circleTowerRadius = 0.7f;
    public int m_circleTowerBulletDamage = 1;
    public float m_circleTowerBulletSpeed = 5f;
    public Color m_circleTowerColor = new Color(0, 0, 0);

    public static EntityFactory Factory;

    private GameObject m_towerPrefab;

    void Awake()
    {
        Factory = this;
    }

    void Start()
    {
        m_towerPrefab = ObjectPool.Pool.m_towerPrefab;
    }

    /// <summary>
    /// towerType = 0 means direct tower, towerType = 1 means circle tower
    /// </summary>
    /// <param name="spawnPos"></param>
    /// <param name="towerType"></param>
    public void CreateTower(Vector3 spawnPos, int towerType)
    {
        var towerObj = ObjectPool.Pool.GetTower();

        if (towerObj != null)
        {
            var tower = towerObj.GetComponent<Tower>();
            var towerSpriteRenderer = towerObj.GetComponent<SpriteRenderer>();
            var attackComp = towerObj.GetComponent<Attack>();

            //removing attack component, since it will be replaced
            //depending on towerType
            Destroy(attackComp);


            if (towerType == 0)
            {
                tower.m_attackRadius = m_directTowerRadius;
                towerSpriteRenderer.color = m_directTowerColor;

                var attack = towerObj.AddComponent<Attack>();
                attack.m_bulletDamage = m_directTowerBulletDamage;
                attack.m_bulletSpeed = m_directTowerBulletSpeed;
                attack.m_bulletColor = m_directTowerColor;
            }

            else if (towerType == 1)
            {
                tower.m_attackRadius = m_circleTowerRadius;
                towerSpriteRenderer.color = m_circleTowerColor;

                var attack = towerObj.AddComponent<CircleAttack>();
                attack.m_bulletDamage = m_circleTowerBulletDamage;
                attack.m_bulletSpeed = m_circleTowerBulletSpeed;
                attack.m_bulletColor = m_circleTowerColor;
            }

            towerObj.transform.position = spawnPos;
            towerObj.SetActive(true);
        }
    }

    public GameObject CreateEnemy(Vector3 spawnPos, int enemyType)
    {
        var enemyObj = ObjectPool.Pool.GetEnemy();

        if (enemyObj != null)
        {
            var enemySpriteRenderer = enemyObj.GetComponent<SpriteRenderer>();
            var enemy = enemyObj.GetComponent<Enemy>();

            if (enemyType == 0)
            {
                enemy.m_speed = m_enemyWeakSpeed;
                enemy.m_startHealth = m_enemyWeakStartHealth;
                enemySpriteRenderer.color = m_enemyWeakColor;
            }

            else if (enemyType == 1)
            {
                enemy.m_speed = m_enemyMediumSpeed;
                enemy.m_startHealth = m_enemyMediumStartHealth;
                enemySpriteRenderer.color = m_enemyMediumColor;
            }

            else if (enemyType == 2)
            {
                enemy.m_speed = m_enemyStrongSpeed;
                enemy.m_startHealth = m_enemyStrongStartHealth;
                enemySpriteRenderer.color = m_enemyStrongColor;
            }

            enemyObj.transform.position = spawnPos;
            enemyObj.SetActive(true);
        }

        return enemyObj;
    }

    public void CreateBullet(Vector3 spawnPos, Vector2 dir, float bulletRadius, float bulletSpeed, float bulletDamage, Color bulletColor)
    {
        var bulletObj = ObjectPool.Pool.GetBullet();

        if (bulletObj != null)
        {
            var bullet = bulletObj.GetComponent<Bullet>();
            var bulletSpriteRenderer = bulletObj.GetComponent<SpriteRenderer>();

            bullet.m_radius = bulletRadius;
            bullet.m_speed = bulletSpeed;
            bullet.m_damage = bulletDamage;
            bullet.SetDirection(dir);

            bulletSpriteRenderer.color = bulletColor;

            bulletObj.transform.position = spawnPos;
            bulletObj.SetActive(true);
        }
    }

    public Sprite GetTowerSprite()
    {
        return m_towerPrefab.GetComponent<SpriteRenderer>().sprite;
    }

    public Vector3 GetTowerScale()
    {
        return m_towerPrefab.transform.localScale;
    }

    public Color GetTowerColor(int towerLevel)
    {
        if (towerLevel == 0)
        {
            return m_directTowerColor;
        }
        else if (towerLevel == 1)
        {
            return m_circleTowerColor;
        }

        //best to return a default value in this case
        return new Color();
    }

    public float GetTowerRadius(int towerLevel)
    {
        if (towerLevel == 0)
        {
            return m_directTowerRadius;
        }
        else if (towerLevel == 1)
        {
            return m_circleTowerRadius;
        }

        return 0;
    }
}

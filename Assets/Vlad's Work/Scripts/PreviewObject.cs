using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    public static PreviewObject Preview;

    private SpriteRenderer m_spriteRend;
    private LineRenderer m_lineRend;
    private int m_circleSegCount = 20;
    private float m_angleStep = 0;

    void Awake()
    {
        Preview = this;
        m_spriteRend = GetComponent<SpriteRenderer>();
        m_lineRend = GetComponent<LineRenderer>();
        m_lineRend.positionCount = m_circleSegCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_angleStep = 360f / m_circleSegCount;
        m_lineRend.loop = true;
    }

    public void GenerateTowerPreview(int towerLevel)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        Vector3Int? spawnTile = GridManager.Grid.GetSpawnLocationTile(hit);

        if (spawnTile != null)
        {
            Vector3 pos = GridManager.Grid.GetCentreOfTile((Vector3Int)spawnTile);

            //draw preview with radius at suggested location
            transform.position = pos;
            transform.localScale = EntityFactory.Factory.GetTowerScale();
            m_spriteRend.sprite = EntityFactory.Factory.GetTowerSprite();
            m_spriteRend.color = EntityFactory.Factory.GetTowerColor(towerLevel);

            float towerRadius = EntityFactory.Factory.GetTowerRadius(towerLevel); 

            for (int i = 0; i < m_circleSegCount; i++){
                m_lineRend.SetPosition(i, ComputeSegment(i*m_angleStep, towerRadius));
            }

            gameObject.SetActive(true);
        }

        else
        {
            //if no spawnTile returned, then the user is not hovering over a valid tile
            //therefore no preview should be shown
            gameObject.SetActive(false);
        }
    }

    private static Vector3 ComputeSegment(float angle, float radius)
    {
        return new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle) * radius, Mathf.Cos(Mathf.Deg2Rad * angle) * radius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Vector2Int m_enemySpawnLocation = new Vector2Int(1, 1);
    public List<Vector2Int> m_pathCorners = new List<Vector2Int>();

    public static GridManager Grid;

    private GameObject m_ground;
    private Grid m_grid;
    private Vector3Int m_enemySpawnTileCoords;


    private Dictionary<Vector3Int, bool> m_occupiedTiles;

    void Awake()
    {
        Grid = this;

        m_ground = GameObject.FindWithTag("Ground");
        m_grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
        m_enemySpawnTileCoords = new Vector3Int(m_enemySpawnLocation.x, m_enemySpawnLocation.y, 0);

        if (m_ground == null)
            Debug.LogError("No object with Ground tag was found!");
        if (m_grid == null)
            Debug.LogError("No Grid object was found!");
    }

    // Start is called before the first frame update
    void Start()
    {
        m_occupiedTiles = new Dictionary<Vector3Int, bool>();
    }

    public GameObject SpawnEnemy(int enemyLevel)
    {
        return EntityFactory.Factory.CreateEnemy(GetCentreOfTile(m_enemySpawnTileCoords), enemyLevel);
    }

    public Vector3Int? GetSpawnLocationTile(RaycastHit2D hit)
    {
        if (hit.collider != null && hit.collider.gameObject == m_ground)
        {
            var tile = m_grid.WorldToCell(hit.point);

            if (!m_occupiedTiles.ContainsKey(tile) && !IsTileOnPath(tile))
            {
                return tile;
            }
        }

        return null;
    }

    public void SpawnTower(RaycastHit2D hit, int towerLevel)
    {
        Vector3Int? spawnTile = GetSpawnLocationTile(hit);

        if(spawnTile != null)
        {
            EntityFactory.Factory.CreateTower(GetCentreOfTile((Vector3Int)spawnTile), towerLevel);
            MarkTileAsOccupied((Vector3Int)spawnTile);
        }
    }

    private void MarkTileAsOccupied(Vector3Int tile)
    {
        m_occupiedTiles.Add(tile, true);
    }

    private bool IsTileOnPath(Vector3Int tile)
    {
        int index = 0;

        while(index < m_pathCorners.Count-1)
        {
            int maxX = Mathf.Max(m_pathCorners[index].x, m_pathCorners[index+1].x);
            int minX = Mathf.Min(m_pathCorners[index].x, m_pathCorners[index + 1].x);

            int maxY = Mathf.Max(m_pathCorners[index].y, m_pathCorners[index + 1].y);
            int minY = Mathf.Min(m_pathCorners[index].y, m_pathCorners[index + 1].y);

            if (minX <= tile.x && tile.x <= maxX &&
               minY <= tile.y && tile.y <= maxY)
            {
                return true;
            }

            index++;
        }

        return false;
    }

    public Vector3 GetCentreOfTile(Vector3 tile)
    {
        var gridPos = m_grid.transform.position;
        var gridSize = m_grid.cellSize;

        var centreOfTile = gridPos + Vector3.Scale(gridSize, tile);
        centreOfTile.x += gridSize.x / 2f;
        centreOfTile.y += gridSize.y / 2f;

        //to ensure object is created above the ground,
        //so that it will be visible
        centreOfTile.z = -1f;

        return centreOfTile;
    }
}

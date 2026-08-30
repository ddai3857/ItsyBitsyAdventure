using Unity.Collections;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    int rows;
    [SerializeField]
    int cols;

    [SerializeField]
    GameObject web_prefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Entity[,] grid;
    Web[,] web_grid;

    Vector2Int spooder_pos;
    void Start()
    {
        grid = new Entity[rows, cols];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Entity Get(Vector2Int grid_pos)
    {
        return grid[grid_pos.x, grid_pos.y];
    }

    //TODO
    public Vector2Int GetGridPos(Vector2 pos)
    {
        return new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
    }

    //TODO
    public Vector2 GetWorldPos(Vector2Int pos)
    {
        return new Vector2(pos.x + 0.5f, pos.y + 0.5f);
    }

    public bool Move(Vector2Int curr_pos, Vector2Int next_pos)
    {
        Entity curr_entity = grid[curr_pos.x, curr_pos.y];
        Entity next_entity = grid[next_pos.x, next_pos.y];

        if (next_entity != null || Vector2Int.Distance(curr_pos, next_pos) > curr_entity.speed)
        {
            return false;
        }

        if (curr_entity is Enemy e && web_grid[curr_pos.x, curr_pos.y] != null)
        {
            if (e.UpdateStuck())
            {
                web_grid[curr_pos.x, curr_pos.y] = null;
            }

            return true;
        }

        StartCoroutine(curr_entity.Walk(GetWorldPos(next_pos)));

        grid[curr_pos.x, curr_pos.y] = curr_entity;

        if (curr_entity is Spooder)
        {
            spooder_pos = next_pos;
        }

        return true;
    }

    public void MoveAllEnemies()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                Entity e = grid[x,y];

                if (e is Enemy)
                {
                    Move(new(x,y), BestEnemyMove(new(x,y)));
                }
            }
        }
    }

    //TODO
    Vector2Int BestEnemyMove(Vector2Int pos)
    {
        return new(-1,-1);
    }

    public bool PlaceWeb(Vector2Int pos)
    {
        Spooder s = grid[spooder_pos.x,spooder_pos.y] as Spooder;
        Entity e = grid[pos.x,pos.y];
        if (Vector2Int.Distance(spooder_pos, pos) > s.web_place_range || e is Obstacle || web_grid[pos.x,pos.y] != null)
        {
            return false;
        }

        web_grid[pos.x,pos.y] = new();

        GameObject web_object = Instantiate(web_prefab);
        Vector2 world_pos = GetWorldPos(pos);
        web_object.transform.position = new(world_pos.x, world_pos.y, 0);

        return true;
    }

    void OnDrawGizmos()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Gizmos.DrawWireCube(GetWorldPos(new(r,c)), Vector3.one);
            }
        }
    }
}

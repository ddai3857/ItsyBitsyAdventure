using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    int rows;
    [SerializeField]
    int cols;

    [SerializeField]
    GameObject web_prefab;

    [SerializeField]
    GameObject select_sprite;

    [SerializeField]
    GameObject camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Entity[,] grid;
    Web[,] web_grid;

    [SerializeField]
    Vector2Int spooder_pos;
    void Start()
    {
        grid = new Entity[rows, cols];
        web_grid = new Web[rows, cols];
        SetCameraPos();
        RemoveSelectSprite();
        SnapChildren();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Entity Get(Vector2Int grid_pos)
    {
        if (grid_pos.x < rows && grid_pos.y < cols)
        {
            return grid[grid_pos.x, grid_pos.y];
        }

        return null;
    }
    public Web GetWeb(Vector2Int grid_pos)
    {
        if (grid_pos.x < rows && grid_pos.y < cols)
        {
            return web_grid[grid_pos.x, grid_pos.y];
        }

        return null;
    }
    public Vector2Int GetGridPos(Vector2 pos)
    {
        return new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
    }
    public Vector2 GetWorldPos(Vector2Int pos)
    {
        return new Vector2(pos.x + 0.5f, pos.y + 0.5f);
    }

    public bool Move(Vector2Int curr_pos, Vector2Int next_pos, bool web)
    {
        Debug.Log(curr_pos);
        Debug.Log(next_pos);
        Debug.Log(Vector2Int.Distance(curr_pos, next_pos));

        Entity curr_entity = grid[curr_pos.x, curr_pos.y];
        Entity next_entity = grid[next_pos.x, next_pos.y];

        if (next_entity != null)
        {
            Debug.Log("ENTITY IS ALREADY THERE");
            return false;
        }

        if (web)
        {
            if (curr_pos.x - next_pos.x != 0 && curr_pos.y - next_pos.y != 0)
            {
                Debug.Log("CAN'T MOVE ENEMIES DIAGONALLY");
                return false;
            }
            if (curr_entity is Enemy e)
            {
                if (web)
                {
                    if (e.IsStuck())
                    {
                        Debug.Log("ENEMY ALREADY STUCK");
                        return false;
                    }

                    e.GetStuck();
                }
                else if (web_grid[curr_pos.x, curr_pos.y] != null && e.UpdateStuck())
                {
                    RemoveWeb(curr_pos);
                    return true;
                }
            } else
            {
                RemoveWeb(curr_pos);
            }
            
        } else if (Vector2Int.Distance(curr_pos, next_pos) > curr_entity.speed)
        {
            Debug.Log("OUT OF RANGE");
            return false;
        }

        curr_entity.Walk(GetWorldPos(next_pos));

        grid[next_pos.x, next_pos.y] = curr_entity;
        grid[curr_pos.x, curr_pos.y] = null;

        if (curr_entity is Spooder)
        {
            spooder_pos = next_pos;
        }

        return true;
    }

    //TODO
    public void MoveAllEnemies()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                Entity e = grid[x,y];

                if (e is Enemy)
                {
                    Move(new(x,y), BestEnemyMove(new(x,y)), false);
                }
            }
        }
    }

    //TODO
    Vector2Int BestEnemyMove(Vector2Int pos)
    {
        return pos;
    }

    public bool PlaceWeb(Vector2Int pos)
    {
        Spooder s = grid[spooder_pos.x,spooder_pos.y] as Spooder;
        Entity e = grid[pos.x,pos.y];
        if (Vector2Int.Distance(spooder_pos, pos) > s.web_place_range || e is Obstacle || web_grid[pos.x,pos.y] != null)
        {
            return false;
        }

        if (!s.PlaceWeb())
        {
            return false;
        }

        GameObject web_object = Instantiate(web_prefab);
        Vector2 world_pos = GetWorldPos(pos);
        web_object.transform.position = new(world_pos.x, world_pos.y, 0);

        web_grid[pos.x,pos.y] = web_object.GetComponent<Web>();

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

    void SnapChildren()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.TryGetComponent(out Entity e))
            {
                Vector2 world_pos = e.transform.position;
                Vector2Int grid_pos = GetGridPos(world_pos);
                e.transform.position = GetWorldPos(grid_pos);
                if (grid[grid_pos.x, grid_pos.y] == null)
                {
                    grid[grid_pos.x, grid_pos.y] = e;
                    if (e is Spooder)
                    {
                        spooder_pos = grid_pos;
                    }
                } else
                {
                    Debug.LogError("TWO ENTITIES IN THE SAME SQUARE");
                }
            }
        }
    }

    public void MoveSelectSprite(Vector2Int grid_pos)
    {
        if (select_sprite.TryGetComponent(out SpriteRenderer s)) {
            s.enabled = true;
        }
        select_sprite.transform.position = GetWorldPos(grid_pos);
    }

    public void RemoveSelectSprite()
    {
        if (select_sprite.TryGetComponent(out SpriteRenderer s)) {
            s.enabled = false;
        }
    }

    void SetCameraPos()
    {
        Vector3 camera_pos = GetWorldPos(new(rows / 2, cols / 2));
        camera.transform.position = new(camera_pos.x - 0.5f, camera_pos.y - 0.5f, -10);
    }

    public bool IsValidPos(Vector2Int pos)
    {
        return 0 <= pos.x && pos.x < rows && 0 <= pos.y && pos.y < cols;
    }

    public void RemoveWeb(Vector2Int pos)
    {
        Destroy(web_grid[pos.x,pos.y].gameObject);
        web_grid[pos.x,pos.y] = null;
    }
}

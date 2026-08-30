using System;
using System.Collections.Generic;
using System.Linq;
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
    Entity[,] entity_grid;
    Obstacle[,] obs_grid;
    Web[,] web_grid;

    [SerializeField]
    Vector2Int spooder_pos;
    void Start()
    {
        entity_grid = new Entity[rows, cols];
        web_grid = new Web[rows, cols];
        obs_grid = new Obstacle[rows, cols];
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
            return entity_grid[grid_pos.x, grid_pos.y];
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
        Entity curr_entity = entity_grid[curr_pos.x, curr_pos.y];

        if (curr_entity == null)
        {
            return false;
        }

        Debug.Log(curr_entity + ": " + curr_pos +", " + next_pos);

        if (!IsValidMovePos(next_pos, curr_entity))
        {
            Debug.Log("ENTITY IS ALREADY THERE OR OBSTACLE BLOCKING");
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

        StartCoroutine(curr_entity.Walk(GetWorldPos(next_pos)));

        entity_grid[next_pos.x, next_pos.y] = curr_entity;
        entity_grid[curr_pos.x, curr_pos.y] = null;

        if (curr_entity is Spooder)
        {
            spooder_pos = next_pos;
        }

        return true;
    }

    bool IsValidMovePos(Vector2Int pos, Entity e)
    {
        if (e is Spooder)
        {
            return entity_grid[pos.x, pos.y] == null && obs_grid[pos.x, pos.y] == null;
        }

        return entity_grid[pos.x, pos.y] is not Enemy && (obs_grid[pos.x, pos.y] == null || e is Birb);
    }

    //TODO
    public void MoveAllEnemies()
    {
        List<Vector2Int> move_list = new();
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                if (entity_grid[x,y] is Enemy)
                {
                    move_list.Add(new(x,y));
                }
            }
        }

        foreach (Vector2Int pos in move_list)
        {
            Move(pos, BestEnemyMove(pos), false);
        }
    }

    //TODO
    Vector2Int BestEnemyMove(Vector2Int pos)
    {
        return AStar(pos, spooder_pos);
    }

    public bool PlaceWeb(Vector2Int pos)
    {
        Spooder s = entity_grid[spooder_pos.x,spooder_pos.y] as Spooder;
        Entity e = entity_grid[pos.x,pos.y];
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
                if (entity_grid[grid_pos.x, grid_pos.y] == null)
                {
                    entity_grid[grid_pos.x, grid_pos.y] = e;
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

    readonly List<Vector2Int> directions = new()
    {
        new(0, 1),
        new(0, -1),
        new(1, 0),
        new(-1, 0),
    };

    public Vector2Int AStar(Vector2Int source, Vector2Int dest)
    {
        Entity e = entity_grid[source.x, source.y];
        // u -> v
        float heuristic(Vector2Int u, Vector2Int v)
        {
            return (u - v).sqrMagnitude;
        }

        // g-value = distance from source to current location
        // h-value = distance from current location to destination
        // f-value = g + h

        // (Location, f-value)
        int sort_func(Tuple<Vector2Int, float> a, Tuple<Vector2Int, float> b)
        {
            int x1 = a.Item2.CompareTo(b.Item2);
            int x2 = a.Item1.x.CompareTo(b.Item1.x);
            int x3 = a.Item1.y.CompareTo(b.Item1.y);

            if (x1 == 0)
            {
                if (x2 == 0)
                {
                    return x3;
                }

                return x2;
            }

            return x1;
        }

        SortedSet<Tuple<Vector2Int, float>> heap = new(Comparer<Tuple<Vector2Int, float>>.Create(sort_func)) { new(source, 0) };

        // Done List
        Dictionary<Vector2Int, int> done = new();

        // (Children,Parent)
        Dictionary<Vector2Int, Vector2Int> node = new();

        // (Location, (g-value, h-value))
        Dictionary<Vector2Int, Tuple<float, float>> values = new() { { source, new(0, 0) } };

        // MAKE A DEEP COPY OF THE ROOM GRID SO WE DONT CHANGE IT IN THE ROOM VARIABLE
        // 0 is walkable, -1 is wall, 1 is path
        // if (room_data.room_grid == null)
        // {
        //     // Debug.Log($"{room_data.grid_size.x}, {room_data.grid_size.y}");

        //     room_data.room_grid = new int[room_data.grid_size.y, room_data.grid_size.x];
        //     foreach (Vector2Int obstacle in room_data.obstacle_locations)
        //     {
        //         // Debug.Log($"{obstacle.x }, {obstacle.y}");
        //         room_data.room_grid[obstacle.y - room_data.grid_position.y, obstacle.x - room_data.grid_position.x] = -1;
        //     }
        // }

        while (heap.Count > 0)
        {
            // We pop the location with the lowest f-value and put it in the done list
            Tuple<Vector2Int, float> first = heap.First();
            Vector2Int parent = first.Item1;

            if (parent == dest)
            {
                break;
            }

            heap.Remove(first);
            done.Add(parent, 0);

            // We check every direction and calculate their f-values
            // If the direction is already in the done list, we skip
            // If the direction is already in the heap with higher f-value, then we update our heap with the current f-value
            // If the direction is already in the heap with lower f-value, then we skip
            foreach (Vector2Int d in directions)
            {
                Vector2Int child = parent + d;

                // If child is a wall/obstacle, we skip
                if (!IsValidPos(child) || !IsValidMovePos(child, e))
                {
                    continue;
                }

                // Removing diagonals that are impossible
                // if (room_data.room_grid[(int)parent.y, (int)child.x] == -1 || room_data.room_grid[(int)child.y, (int)parent.x] == -1)
                // {
                //     continue;
                // }

                float new_g = values[parent].Item1 + heuristic(parent, child);
                float new_h = heuristic(child, dest);

                if (values.ContainsKey(child))
                {
                    float old_f = values[child].Item1 + values[child].Item2;

                    if (done.ContainsKey(child) || old_f <= new_g + new_h)
                    {
                        // Debug.Log($"CO0NTINUED: {d}");
                        continue;
                    }

                    heap.Remove(new Tuple<Vector2Int, float>(child, old_f));
                    values[child] = new Tuple<float, float>(new_g, new_h);
                }
                else
                {
                    values.Add(child, new Tuple<float, float>(new_g, new_h));
                }

                if (node.ContainsKey(child))
                {
                    node[child] = parent;
                }
                else
                {
                    node.Add(child, parent);
                }

                heap.Add(new Tuple<Vector2Int, float>(child, new_g + new_h));

            }
        }

        List<Vector2Int> ans = new();

        Vector2Int curr = dest;
        while (curr != source)
        {
            ans.Add(curr);
            if (!node.ContainsKey(curr))
            {
                Debug.LogError($"{curr} and {dest} \n");
                break;
            }
            else
            {
                curr = node[curr];
            }
        }

        return ans.Last();
    }
}

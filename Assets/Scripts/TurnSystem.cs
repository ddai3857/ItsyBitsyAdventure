using UnityEngine;
using UnityEngine.InputSystem;

public class TurnSystem : MonoBehaviour
{
    int curr_turn = 0;
    [SerializeField]
    Entity selected_entity = null;
    [SerializeField]
    Web selected_web = null;
    [SerializeField]
    Vector2Int selected_pos = new(-1,-1);
    InputAction interact;
    InputAction web;

    [SerializeField]
    Board grid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interact = InputSystem.actions.FindAction("Interact");
        web = InputSystem.actions.FindAction("Web");
    }

    // Update is called once per frame
    void Update()
    {
        if (interact.WasPressedThisFrame())
        {
            Vector2Int grid_pos = GetMouseGridPos();

            if (grid.IsValidPos(grid_pos))
            {
                grid.MoveSelectSprite(grid_pos);

                if (HandleInteraction(grid_pos))
                {
                    EndTurn();
                }
            }
        }

        if (web.WasPressedThisFrame())
        {
            Vector2Int grid_pos = GetMouseGridPos();

            if (grid.IsValidPos(grid_pos))
            {
                grid.PlaceWeb(grid_pos);
            }
        }
    }

    Vector2Int GetMouseGridPos()
    {
        Vector3 screen_pos = Mouse.current.position.ReadValue();
        screen_pos.z = 10f;
        Vector3 world_pos = Camera.main.ScreenToWorldPoint(screen_pos);
        return grid.GetGridPos(world_pos);
    }

    bool HandleInteraction(Vector2Int grid_pos)
    {
        Entity new_selected = grid.Get(grid_pos);
        Web new_web = grid.GetWeb(grid_pos);
        if (selected_entity is Spooder)
        {
            if (!grid.Move(selected_pos, grid_pos))
            {
                RemoveSelection();
                return false;
            }

            return true;
        }

        if (selected_web != null)
        {
            if (!grid.WebMove(grid_pos, selected_pos))
            {
                RemoveSelection();
                return false;
            }

            return true;
        }

        selected_web = new_web;
        selected_entity = new_selected;
        selected_pos = grid_pos;
        return false;
    }

    void EndTurn()
    {
        RemoveSelection();
        grid.MoveAllEnemies();
    }

    void RemoveSelection()
    {
        grid.RemoveSelectSprite();
        selected_entity = null;
        selected_web = null;
        selected_pos = new(-1,-1);   
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class TurnSystem : MonoBehaviour
{
    int curr_turn = 0;
    Entity selected_entity = null;
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
            Vector3 screen_pos = Mouse.current.position.ReadValue();
            screen_pos.z = 10f;
            Vector3 world_pos = Camera.main.ScreenToWorldPoint(screen_pos);
            Vector2Int grid_pos = grid.GetGridPos(world_pos);

            grid.MoveSelectSprite(grid_pos);

            if (HandleInteraction(grid_pos))
            {
                EndTurn();
            }
        }
    }

    bool HandleInteraction(Vector2Int grid_pos)
    {
        Entity new_selected = grid.Get(grid_pos);
        if (selected_entity == null)
        {
            selected_entity = new_selected;
            selected_pos = grid_pos;
            return false;
        }

        if (selected_entity is Spooder)
        {
            return grid.Move(selected_pos, grid_pos);
        }

        //HANDLE WEBS

        return false;
    }

    void EndTurn()
    {
        grid.RemoveSelectSprite();
        selected_entity = null;
        selected_pos = new(-1,-1);
        grid.MoveAllEnemies();
    }
}

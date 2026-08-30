using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TurnSystem : MonoBehaviour
{
    int curr_turn = 1;
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

    [SerializeField]
    GameObject spooder_obj;

    public SceneFade screen_fade;

    public bool can_interact = true;
    public bool game_ended = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interact = InputSystem.actions.FindAction("Interact");
        web = InputSystem.actions.FindAction("Web");
    }

    // Update is called once per frame
    void Update()
    {
        if (!can_interact || game_ended)
        {
            return;
        }
        
        if (interact.WasPressedThisFrame())
        {
            Vector2Int grid_pos = GetMouseGridPos();

            if (grid.IsValidPos(grid_pos))
            {
                grid.MoveSelectSprite(grid_pos);
                can_interact = false;
                StartCoroutine(HandleInteraction(grid_pos));
            }
        }

        if (web.WasPressedThisFrame())
        {
            Vector2Int grid_pos = GetMouseGridPos();

            if (grid.IsValidPos(grid_pos))
            {
                grid.PlaceWeb(grid_pos);
                RemoveSelection();
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

    IEnumerator HandleInteraction(Vector2Int grid_pos)
    {
        Entity new_selected = grid.Get(grid_pos);
        Web new_web = grid.GetWeb(grid_pos);
        if (selected_entity is Spooder)
        {
            yield return StartCoroutine(grid.Move(selected_pos, grid_pos));
            if (!grid.CheckMoveStatus())
            {
                RemoveSelection();
                can_interact = true;
                yield break;
            }
        }else if (selected_web != null)
        {
            yield return StartCoroutine(grid.WebMove(grid_pos, selected_pos));
            if (!grid.CheckMoveStatus())
            {
                RemoveSelection();
                can_interact = true;
                yield break;
            }
        } else
        {
            selected_web = new_web;
            selected_entity = new_selected;
            selected_pos = grid_pos;
            can_interact = true;
            yield break;
        }

        yield return StartCoroutine(EndTurn());
        can_interact = true;
    }

    IEnumerator EndTurn()
    {
        RemoveSelection();
        spooder_obj.GetComponent<Spooder>().UpdateTimer();
        yield return StartCoroutine(grid.MoveAllEnemies());
        curr_turn += 1;
    }

    void RemoveSelection()
    {
        grid.RemoveSelectSprite();
        selected_entity = null;
        selected_web = null;
        selected_pos = new(-1,-1);   
    }

    //TODO
    public void LoseGame()
    {
        Debug.Log("YOU LOSE!");
        game_ended = true;
        screen_fade.EaseOut(spooder_obj.transform.position,1).OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    //TODO
    public void WinGame()
    {
        Debug.Log("YOU WIN!");
        game_ended = true;

        Levels.curr_unlock++;
        if (Levels.curr_level > 6)
        {
            screen_fade.EaseOut(spooder_obj.transform.position,1).OnComplete(() => SceneManager.LoadScene("Levels"));
        }
        else
        {
            screen_fade.EaseOut(spooder_obj.transform.position,1).OnComplete(() => SceneManager.LoadScene("Level" + (++Levels.curr_level)));
        }
    }
}

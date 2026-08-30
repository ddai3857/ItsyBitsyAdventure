using UnityEngine;

public class Enemy : Entity
{
    [SerializeField]
    int stuck_turns;
    int curr_stuck = 0;

    public bool UpdateStuck()
    {
        curr_stuck -= 1;

        if (curr_stuck == 0)
        {
            return true;
        }

        return false;
    }

    public virtual void GetStuck()
    {
        curr_stuck = stuck_turns;
    }

    public bool IsStuck()
    {
        return curr_stuck > 0;
    }
}

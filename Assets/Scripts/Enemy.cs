using UnityEngine;

public class Enemy : Entity
{
    [SerializeField]
    int stuck_turns;
    int curr_stuck = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool UpdateStuck()
    {
        curr_stuck -= 1;

        if (curr_stuck == 0)
        {
            return true;
        }

        return false;
    }

    public void GetStuck()
    {
        curr_stuck = stuck_turns;
    }

    public bool IsStuck()
    {
        return curr_stuck > 0;
    }
}

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
        curr_stuck += 1;

        if (curr_stuck == stuck_turns)
        {
            curr_stuck = 0;
            return true;
        }

        return false;
    }
}

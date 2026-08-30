using System.Collections;
using UnityEngine;

public class Spooder : Entity
{
    [SerializeField]
    int max_web_charge;
    [SerializeField]
    int web_charge;
    [SerializeField]
    int web_cooldown;
    int web_timer = 0;

    [SerializeField]
    GameObject turn_system_obj;

    [SerializeField]
    public int web_place_range;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTimer()
    {
        web_timer += 1;

        if (web_timer == web_cooldown)
        {
            web_charge = Mathf.Min(max_web_charge, web_charge + 1);
            web_timer = 0;
        }
    }

    public bool PlaceWeb()
    {
        if (web_charge > 0)
        {
            web_charge--;
            return true;
        }
        return false;
    }

    //TODO
    public void GetEaten()
    {
        turn_system_obj.GetComponent<TurnSystem>().LoseGame();
    }

    //TODO
    public new virtual IEnumerator Squish()
    {
        turn_system_obj.GetComponent<TurnSystem>().LoseGame();
        yield return StartCoroutine(base.Squish());
    }

    //TODO
    public new virtual IEnumerator Shrink()
    {
        turn_system_obj.GetComponent<TurnSystem>().LoseGame();
        yield return StartCoroutine(base.Shrink());
    }
}

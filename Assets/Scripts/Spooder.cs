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
    public WebUI web_ui;

    public void Start()
    {
        StartCoroutine(SpriteFlipLoop());
        web_ui.UpdateWebCount(web_charge);
    }
    public void UpdateTimer()
    {
        if (max_web_charge == web_charge)
        {
            web_timer = 0;
            return;
        }

        web_timer += 1;

        if (web_timer == web_cooldown)
        {
            web_charge = Mathf.Min(max_web_charge, web_charge + 1);
            web_ui.UpdateWebCount(web_charge);

            web_timer = 0;
        }
        web_ui.UpdateWebReload(web_timer);
    }

    public bool PlaceWeb()
    {
        if (web_charge > 0)
        {
            web_charge--;
            web_ui.UpdateWebCount(web_charge);
            return true;
        }
        return false;
    }

    //TODO
    public void GetEaten()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        turn_system_obj.GetComponent<TurnSystem>().LoseGame();
    }

    //TODO
    public override IEnumerator Squish()
    {
        turn_system_obj.GetComponent<TurnSystem>().LoseGame();
        yield return StartCoroutine(base.Squish());
    }

    //TODO
    public override IEnumerator Shrink()
    {
        turn_system_obj.GetComponent<TurnSystem>().LoseGame();
        yield return StartCoroutine(base.Shrink());
    }
}

using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Birb : Enemy
{
    int flying_speed = 2;
    int walking_speed = 1;

    [SerializeField]
    Sprite walking_sprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = flying_speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public new void GetStuck()
    {
        GetComponent<SpriteRenderer>().sprite = walking_sprite;
        speed = walking_speed;

        base.GetStuck();
    }
}

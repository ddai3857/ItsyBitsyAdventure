using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Birb : Enemy
{
    [SerializeField]
    Sprite walking_sprite;

    bool is_flying = true;

    public bool IsFlying()
    {
        return is_flying;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void GetStuck()
    {
        SpriteRenderer sprite_renderer = GetComponent<SpriteRenderer>();
        sprite_renderer.sprite = walking_sprite;
        sprite_renderer.sortingOrder = 2;
        is_flying = false;
        base.GetStuck();
    }
}

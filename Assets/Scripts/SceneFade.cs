using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneFade : MonoBehaviour
{
    public float load_speed = 0.5F;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EaseIn(load_speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Tween EaseIn(float speed = 0.5F)
    {
        return transform.DOScale(new Vector3(0,0,0), speed)
            .SetEase(Ease.InSine);
    }

    public Tween EaseOut(Vector3 pos, float speed = 0.5F)
    {
        transform.position = pos;
        return transform.DOScale(new Vector3(20,20,20), speed)
            .SetEase(Ease.OutSine);
    }
}

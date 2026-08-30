using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneFade : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EaseIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Tween EaseIn()
    {
        return transform.DOScale(new Vector3(0,0,0), .5F)
            .SetEase(Ease.InSine);
    }

    public Tween EaseOut(Vector3 pos)
    {
        transform.position = pos;
        return transform.DOScale(new Vector3(20,20,20), .5F)
            .SetEase(Ease.OutSine);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class VolumeMenu : MonoBehaviour
{
    Slider slide;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slide = GetComponent<Slider>();
        slide.value = PersistentAudio.instance.GetComponent<AudioSource>().volume;
    }

    public void UpdateSlider()
    {
        PersistentAudio.instance.GetComponent<AudioSource>().volume = slide.value;
    }
}

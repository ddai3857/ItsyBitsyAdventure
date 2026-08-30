using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{
    public List<Sprite> nums = new List<Sprite>(6);
    public Sprite cover;
    public static int curr_level = 1;
    public static int curr_unlock = 1;
    public static int max_level = 10;
    public SceneFade screen_fade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int idx = 0;
        foreach (Transform child in transform)
        {
            if (idx < curr_unlock)
            {
                child.GetComponent<Image>().sprite = nums[idx];
                child.GetComponent<Button>().enabled = true;
                child.GetComponent<EventTrigger>().enabled = true;
            }
            else
            {
                child.GetComponent<Image>().sprite = cover;
                child.GetComponent<Button>().enabled = false;
                child.GetComponent<EventTrigger>().enabled = false;
            }
            idx++;
        }
    }

    public void LoadLevel(int idx)
    {
        curr_level = idx;
        screen_fade.EaseOut(transform.GetChild(idx-1).position).OnComplete(() => SceneManager.LoadScene("Level" + idx));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

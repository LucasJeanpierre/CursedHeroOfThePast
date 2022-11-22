using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeEffect : MonoBehaviour
{

    public TimeManager timeManager;
    private float time;
    private SpriteRenderer spriteRenderer;
    private Color spriteColor;
    private float hue;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteColor.a = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        time = timeManager.GetTime();
        ChangeColorAccordingTime(time);
    }

    /**
     * The color of the main Character change according to the time
     * The Default color for time 0 is yellow
     * If the Character goes into the futur, he became greenish
     * If the Character goes into the past, he became redish
     * */
    private void ChangeColorAccordingTime(float time)
    {
        hue = 0.166f;
        hue += time / 100;

        if (hue < 0)
        {
            hue = 0;
        } else if (hue > 0.4f)
        {
            hue = 0.4f;
        }
        spriteColor = Color.HSVToRGB(hue, 1, 1);
        spriteRenderer.color = spriteColor;
    }
}

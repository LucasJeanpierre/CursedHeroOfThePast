using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{

    private MovingPartSlider _movingPartSlider;
    private float _sliderWidth;

    public float _openingAngle = 360f;

    // Start is called before the first frame update
    void Start()
    {
        _movingPartSlider = this.GetComponentInChildren<MovingPartSlider>();
        _sliderWidth = this.GetComponent<SpriteRenderer>().bounds.size.x;
        
    }

    public float getSliderWidth() {
        return _sliderWidth;
    }
    // Update is called once per frame
    void Update()
    {
    }

    public float getRotation(){
        return (_movingPartSlider.getSliderPosition()*_openingAngle);
    }
}

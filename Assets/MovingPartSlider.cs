using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPartSlider : MonoBehaviour
{

    private Transform _transform;
    private Rigidbody2D _rigidbody2D;
    private bool outOfSliderBounds;


    private Slider _slider;    
    [SerializeField] private float forceMagnitude = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        _slider = this.GetComponentInParent<Slider>();
        outOfSliderBounds = false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(!outOfSliderBounds){
            Rigidbody2D otherRigidbody2D = other.collider.GetComponent<Rigidbody2D>();
            if(otherRigidbody2D != null) {
                Vector2 forceDirection = other.gameObject.transform.position - _transform.position;
                forceDirection.y =0;
                forceDirection.Normalize();
                Debug.Log(forceDirection);
                _rigidbody2D.velocity = forceDirection * forceMagnitude;
                //_rigidbody2D.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float _sliderWidth = _slider.getSliderWidth();
        bool outOfSliderBounds = (_transform.position.x > _slider.transform.position.x + _sliderWidth/2)||( _transform.position.x < _slider.transform.position.x - _sliderWidth/2);
        if(outOfSliderBounds) {
            _rigidbody2D.velocity = Vector2.zero;
        }
    }
}

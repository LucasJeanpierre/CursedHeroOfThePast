using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPartSlider : MonoBehaviour
{

    private Transform _transform;
    private Rigidbody2D _rigidbody2D;

    private BoxCollider2D _boxCollider2D;
    private bool outOfSliderBounds;

    [SerializeField] private TimeManager _timeManager;

    private SpriteRenderer _movingPartSpriteRenderer;


    private Slider _slider;    
    [SerializeField] private float forceMagnitude = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _slider = this.GetComponentInParent<Slider>();
        outOfSliderBounds = false;
        _movingPartSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // private void OnCollisionEnter2D(Collision2D other) {
    //     if(!outOfSliderBounds){
    //         Rigidbody2D otherRigidbody2D = other.collider.GetComponent<Rigidbody2D>();
    //         if(otherRigidbody2D != null) {
    //             Vector2 forceDirection = other.gameObject.transform.position - _transform.position;
    //             forceDirection.y =0;
    //             forceDirection.Normalize();
    //             Debug.Log(forceDirection);
    //             _rigidbody2D.velocity = forceDirection * forceMagnitude;
    //             //_rigidbody2D.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);
    //         }
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        _rigidbody2D.velocity = Vector2.zero;
        float _sliderWidth = _slider.getSliderWidth();
        bool outOFBoundsLeft = _transform.position.x < _slider.transform.position.x - _sliderWidth/2;
        bool outOFBoundsRight = _transform.position.x > _slider.transform.position.x + _sliderWidth/2;
        if (outOFBoundsLeft) {
            _transform.position = new Vector3(_slider.transform.position.x - _sliderWidth/2, _transform.position.y, _transform.position.z);
            _rigidbody2D.velocity = Vector2.zero;
            StartCoroutine("CoolDownCollisionEnable");
        }
        else if (outOFBoundsRight) {
            _transform.position = new Vector3(_slider.transform.position.x + _sliderWidth/2, _transform.position.y, _transform.position.z);
            _rigidbody2D.velocity = Vector2.zero;
            StartCoroutine("CoolDownCollisionEnable");
        }

        // if (_timeManager.isRewinding()) {
        //     _movingPartSpriteRenderer.color = Color.white;
        // } else {
        //     _movingPartSpriteRenderer.color = _slider.GetComponent<SpriteRenderer>().color;
        // }
    }

    IEnumerator CoolDownCollisionEnable() {
        _boxCollider2D.enabled = false;
        yield return new WaitForSeconds(0.1f);
        _boxCollider2D.enabled = true;
    }

    public float getSliderPosition() {
        return transform.localPosition.x;
    }
}

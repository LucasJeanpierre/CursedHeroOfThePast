

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    
    
    private bool _isAbove = false;
    private bool _left = true;

    private bool _isPressed = false;
    


    private Animator _animator;


    // private int nbcollision = 0;
    [SerializeField] private float _buttonDelayOnRelease = 4f;

    
    
    // Start is called before the first frame update
    void Start()
    {
        _animator= GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
         PressButton();
        
    }

    public void PressButton()
    {
        _isAbove=true;
        _left=false;
        // GetComponent<SpriteRenderer>().color = Color.red;
        _animator.SetBool("Someone_Above",_isAbove);
        _animator.SetBool("Someone_Left",_left);
    }



    private void OnCollisionExit2D(Collision2D other)

    {
        _isAbove=false;
        _animator.SetBool("Someone_Above",_isAbove);
        // Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine("unPressButtonAfterDelay");
        }
        StartCoroutine("unPressButtonAfterDelay");
        //  isPressed=false;
        // _animator.SetBool("Someone_Above",isPressed );
    }

    public bool getButtonState()
    {
        return (_isPressed );
    }

    // Update is called once per frame
    void Update()
    {
        // UpdateCollider();
        // Debug.Log(isPressed);
        _isPressed= !(_left);
        _animator.SetBool("Button_Pressed",_isPressed);
    }

    public void unPressButton()
    {
        
        // GetComponent<SpriteRenderer>().color = Color.blue;
        if (!_isAbove){
            _left=true;
            _animator.SetBool("Someone_Left",_left);
        }
    }

    public IEnumerator unPressButtonAfterDelay()
    {
        yield return new WaitForSeconds(_buttonDelayOnRelease);

        unPressButton();
    }

    

    


    void UpdateCollider(){
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        UnityEngine.Bounds bounds = spriteRenderer.sprite.bounds;
        Vector2 S = bounds.size;

        BoxCollider2D boxCollider=gameObject.GetComponent<BoxCollider2D>();

        
        float offset_y=S.y/2;
        // if (S.y<=0.2){
        //     offset_y=6000;
        // }

        // float x_dim=boxCollider.size.x;
        boxCollider.size=new Vector2(S.x,S.y);
        boxCollider.offset=new Vector2 (0,offset_y );
    }


    
}

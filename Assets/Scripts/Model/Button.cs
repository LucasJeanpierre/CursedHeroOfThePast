

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
    [SerializeField] private float _buttonDelayOnRelease ;

    
    
    // Start is called before the first frame update
    void Awake()
    {
        _animator= GetComponent<Animator>();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
         Collider2D playerCollider = other.gameObject.GetComponent<Collider2D>();
         _isAbove= collideAbove(playerCollider);
         _left=false;
         if (_isAbove){
            PressButton();
         }
        
    }

   

    public void PressButton()
    {
        
        _isPressed=true;
        _animator.SetBool("Button_Pressed",_isPressed);
        StopCoroutine("unPressButtonAfterDelay");
        // GetComponent<SpriteRenderer>().color = Color.red;
        _animator.SetBool("Someone_Above",_isAbove);
        _animator.SetBool("Someone_Left",_left);
    }



    private void OnCollisionExit2D(Collision2D other)

    {
        _isAbove=false;
        _animator.SetBool("Someone_Above",_isAbove);
        // Debug.Log(other.gameObject.tag);
        // if (other.gameObject.tag == "Player")
        // {
        //     StartCoroutine("unPressButtonAfterDelay");
        // }
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
        UpdateCollider();
        // Debug.Log(_isPressed);
        
    }

    public void unPressButton()
    {
        
        // GetComponent<SpriteRenderer>().color = Color.blue;
        // if (!_isAbove){
            _isPressed=false;
            _animator.SetBool("Button_Pressed",_isPressed);
            _left=true;
            _animator.SetBool("Someone_Left",_left);
        // }
        
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

    private bool collideAbove(Collider2D playerCollider){

        
        float y_inf_player=playerCollider.bounds.min.y;

        BoxCollider2D button_collider= this.gameObject.GetComponent<BoxCollider2D>();
        float y_max_button=button_collider.bounds.max.y;

        if (y_max_button<=y_inf_player){
            return true;
        }
        return false;

        


    }


    
}

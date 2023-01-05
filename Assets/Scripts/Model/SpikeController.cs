using System;
using System.Collections;
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.SceneManagement;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Gameplay;
using Platformer.Model;
using static Platformer.Core.Simulation;


public class SpikeController : MonoBehaviour
{
    internal Animator _animator;
    [SerializeField] bool _toAnimate;
    [SerializeField] float _uppingSpeed;
    [SerializeField] float _downingSpeed;
    [SerializeField] float _upPause;
    [SerializeField] float _downPause;


    // Start is called before the first frame update
    void Start()
    {
        _animator= GetComponent<Animator>();
        if (_toAnimate){
            _animator.SetBool("Animated",true);
        }

        _animator.SetFloat("upPause",(float) 0.1/_upPause);
        _animator.SetFloat("downPause",(float) 0.1/_downPause);
        _animator.SetFloat("downingSpeed",_downingSpeed);
        _animator.SetFloat("uppingSpeed",_uppingSpeed);
    }

    

    // Update is called once per frame
    void Update()
    {
        if (_toAnimate){
            UpdateCollider();
        }
        
        
        
    }

    void UpdateCollider(){
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        UnityEngine.Bounds bounds = spriteRenderer.sprite.bounds;
        Vector2 S = bounds.size;

        BoxCollider2D boxCollider=gameObject.GetComponent<BoxCollider2D>();

        
        float offset_y=S.y/2;
        if (S.y<=0.2){
            offset_y=6000;
        }

        // float x_dim=boxCollider.size.x;
        boxCollider.size=new Vector2(S.x,S.y);
        boxCollider.offset=new Vector2 (0,offset_y );
    }


 public void OnCollisionEnter2D(){
    Schedule<PlayerDeath>();
 }
    
}

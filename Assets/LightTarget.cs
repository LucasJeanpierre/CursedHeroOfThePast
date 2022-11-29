using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTarget : Button
{

    Coroutine _lightCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnpressButtonAfterDelay()
    {
        //_lightCoroutine = Button.UnpressButtonAfterDelay();
        unPressButton();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
    }
}

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

    public void unPressLightRayTargetAfterDelay()
    {
        //_lightCoroutine = Button.UnpressButtonAfterDelay();
        //base.unPressButton();
        StartCoroutine("unPressLightRayTarget");
        //yield return new WaitForSeconds(base.getButtonDelayOnRelease());
    }

    public IEnumerator unPressLightRayTarget()
    {
        yield return new WaitForSeconds(base.getButtonDelayOnRelease());
        base.unPressButton();
        this.getAnimator().SetBool("Someone_Above", false);
        this.getAnimator().SetBool("Button_Pressed", false);
        this.getAnimator().SetBool("Someone_Left", true);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {

    }
}

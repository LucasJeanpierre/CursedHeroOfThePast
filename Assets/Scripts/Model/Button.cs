

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    
    
    [SerializeField] private bool isPressed = false;


    // private int nbcollision = 0;
    [SerializeField] private float _buttonDelayOnRelease = 4f;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PressButton();
    }

    public void PressButton()
    {
        isPressed = true;
        GetComponent<SpriteRenderer>().color = Color.red;
    }



    private void OnCollisionExit2D(Collision2D other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine("unPressButtonAfterDelay");
        }
        //StartCoroutine("unPressButtonAfterDelay");
    }

    public bool getButtonState()
    {
        return isPressed;
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void unPressButton()
    {
        isPressed = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public IEnumerator unPressButtonAfterDelay()
    {
        yield return new WaitForSeconds(_buttonDelayOnRelease);
        unPressButton();
    }
}

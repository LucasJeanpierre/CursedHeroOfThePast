

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Button otherButton;
    public Door door;
    private bool isPressed = false;
    private int nbcollision = 0;
    [SerializeField] private float _buttonDelayOnRelease = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        isPressed = true;
        GetComponent<SpriteRenderer>().color = Color.red;
        nbcollision++;
    }



    private void OnCollisionExit2D(Collision2D other)
    {
        nbcollision--;
        if (nbcollision == 0)
        {
            StartCoroutine("unPressButtonAfterDelay");
        }
    }

    public bool getButtonState()
    {
        return isPressed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressed && otherButton.getButtonState())
        {
            door.open();
        }
    }

    private void unPressButton()
    {
        isPressed = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    IEnumerator unPressButtonAfterDelay()
    {
        yield return new WaitForSeconds(_buttonDelayOnRelease);
        unPressButton();
    }
}

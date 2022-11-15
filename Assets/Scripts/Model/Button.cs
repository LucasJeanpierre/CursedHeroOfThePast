

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Button otherButton;
    public Door door;
    public bool isPressed = false;
    private int nbcollision = 0;
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

            isPressed = false;
            GetComponent<SpriteRenderer>().color = Color.white;
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

    IEnumerator Wait()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);
    }
}

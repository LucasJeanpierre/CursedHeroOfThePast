


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject door;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void open()
    {
        door.GetComponent<Renderer>().enabled = false;
        door.GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

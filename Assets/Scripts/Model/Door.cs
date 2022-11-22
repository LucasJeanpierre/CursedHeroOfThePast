


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // public GameObject door;
    // Start is called before the first frame update
    [SerializeField] List<Button> _buttonList= new List<Button>{};

    private bool _toBeOpen=false;
    

    void Start()
    {

    }

    public void open()
    {
        this.GetComponent<Renderer>().enabled = false;
        this.GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        _toBeOpen=true;
        foreach (Button b in _buttonList){
            if (! b.getButtonState()){
                _toBeOpen=false;
            }
        }
        

        if (_toBeOpen){
            open();
        }

    }
}

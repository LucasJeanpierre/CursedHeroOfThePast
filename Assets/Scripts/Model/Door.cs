using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Door : MonoBehaviour
{
    public AudioClip openDoor;
    public AudioSource audioSource;

    [SerializeField] CinemachineTargetGroup _cinemachineTargetGroup;
    private Transform _transform;
    private float _originalY;

    // public GameObject door;
    // Start is called before the first frame update
    [SerializeField] List<Button> _buttonList= new List<Button>{};

    public float cameraSpeed = 0.005f;

    public bool _toBeOpen=false;
    public bool _isMiddleDoor=false;
    private bool _doorStateToBeChanged=false;
    private int _buttonPressed=-1;

    private bool _moveCameraToDoor=false;
    private bool _moveCameraFromDoor=false;
    private float _weightValue=0f;

    private bool _animateOpenDoor=false;
    private bool _animateCloseDoor=false;
    private float _animateDoorSpeed=0.02f;

    void Start()
    {
        _transform = this.transform;
        _originalY = _transform.position.y;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (_toBeOpen) open();
    }

    public void open()
    {
        //Color tmp = this.GetComponent<SpriteRenderer>().color;
        //tmp.a = .75f;
        //this.GetComponent<SpriteRenderer>().color = tmp;
        _animateOpenDoor = true;
        _animateCloseDoor = false;
        this.GetComponent<Collider2D>().enabled = false;
    }

    public void close()
    {
        _animateOpenDoor = false;
        _animateCloseDoor = true;
        this.GetComponent<Collider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        _toBeOpen=true;
        foreach (Button b in _buttonList){
            if (! b.getButtonState()){ // Step out of button
                _toBeOpen=false;
                if (_buttonList.IndexOf(b) == _buttonPressed){
                    _buttonPressed = -1;
                    _moveCameraToDoor = false;
                    _moveCameraFromDoor = true;
                }
                _doorStateToBeChanged = true;
            }
            else{ // Step on button
                if (_buttonPressed == -1){
                    _cinemachineTargetGroup.AddMember(_transform, 0f, 0f);
                    _moveCameraToDoor = true;
                    _moveCameraFromDoor = false;
                }
                _buttonPressed = _buttonList.IndexOf(b);
            }
        }
        if (_toBeOpen && _doorStateToBeChanged){
            if (_isMiddleDoor)
            {
                if (_transform.position.y <= _originalY) // Door closed
                {
                    open();
                    if (this.audioSource && this.openDoor)
                        this.audioSource.PlayOneShot(this.openDoor);
                }
                else // Door opened
                {
                    close();
                    if (this.audioSource && this.openDoor)
                        this.audioSource.PlayOneShot(this.openDoor);
                }
            }
            else
            {
                open();
                if (this.audioSource && this.openDoor)
                    this.audioSource.PlayOneShot(this.openDoor);
            }
            _doorStateToBeChanged = false;
        }

        if (_moveCameraToDoor){
            var doorTargetNumber = _cinemachineTargetGroup.FindMember(_transform);
            _cinemachineTargetGroup.m_Targets[doorTargetNumber].weight = _weightValue;
            _weightValue += cameraSpeed;
            if (_weightValue >= 1f) _weightValue = 1f;
        }
        if (_moveCameraFromDoor){
            var doorTargetNumber = _cinemachineTargetGroup.FindMember(_transform);
            if (doorTargetNumber == -1) return; // If door is not in the target group (because it was removed)
            _cinemachineTargetGroup.m_Targets[doorTargetNumber].weight = _weightValue;
            if (_cinemachineTargetGroup.m_Targets[doorTargetNumber].weight == 0f)
                _cinemachineTargetGroup.RemoveMember(_transform);
            _weightValue -= cameraSpeed;
            if (_weightValue <= 0f) _weightValue = 0f;
        }

        if (_animateOpenDoor)
        {
            _transform.position += new Vector3(0, _animateDoorSpeed, 0);
            if (_transform.position.y > _originalY + 1.6)
                _animateOpenDoor = false;
        }
        if (_animateCloseDoor)
        {
            _transform.position -= new Vector3(0, _animateDoorSpeed, 0);
            if (_transform.position.y <= _originalY)
                _animateCloseDoor = false;
        }
    }
}

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

    // public GameObject door;
    // Start is called before the first frame update
    [SerializeField] List<Button> _buttonList= new List<Button>{};

    public float cameraSpeed = 0.005f;

    private bool _toBeOpen=false;
    private int _buttonPressed=-1;
    private bool _moveCameraToDoor=false;
    private bool _moveCameraFromDoor=false;
    private float _weightValue=0f;

    void Start()
    {
        _transform = this.transform;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
            if (! b.getButtonState()){ // Step out of button
                _toBeOpen=false;
                if (_buttonList.IndexOf(b) == _buttonPressed){
                    _buttonPressed = -1;
                    _moveCameraToDoor = false;
                    _moveCameraFromDoor = true;
                }
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
        

        if (_toBeOpen){
            open();
            if (this.audioSource && this.openDoor)
                this.audioSource.PlayOneShot(this.openDoor);
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

    }
}

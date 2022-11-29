using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{

    [SerializeField] private float _time = 0.0f;

    [SerializeField] private float _timeRewindLimit = 10.0f;

    private float _maxTime = 0.0f;

    private bool _onRewind = false;


    private float _gameCustomTime = 0.0f;

    [SerializeField] private List<GameObject> _ListOfRewindObjects;
    //public Text TimeText;

    public void FixedUpdate() {
        if (!_onRewind) {
            _gameCustomTime += Time.fixedDeltaTime;
        } else {
            _gameCustomTime -= Time.fixedDeltaTime;
        }

        if (_gameCustomTime > _maxTime) {
            _maxTime = _gameCustomTime;
        }

        _gameCustomTime = (float) System.Math.Round(_gameCustomTime, 2);

        //Debug.Log(_gameCustomTime);
    }

    

    // Update is called once per frame
    void Update()
    {
        //Display the current time on the UI
        //TimeText.text = "Time : " + Time.ToString();

        //Change the time manually
        #if (UNITY_EDITOR)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _time += 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _time -= 1.0f;
        }
        #endif
    }

    public void RewindAllAffectedObjects()
    {
        Debug.Log("rewind all objects  " + _gameCustomTime);
        foreach (var rewindObject in _ListOfRewindObjects)
        {
            rewindObject.GetComponent<RewindSaveInfo>().RewindTo(_gameCustomTime);
        }
        //SetCustomTime(rewind_time);
        //_gameCustomTime = rewind_time;
    }

    public float GetCustomTime() {
        return _gameCustomTime;
    }

    public void SetCustomTime(float new_time) {
        _gameCustomTime = new_time;
    }

    /**
     * This is the function that allow all the other objects of the game the get the current time
     * The time variable is private to prevent other object from modifying it
     * */
    public float GetTime()
    {
        return _time;
    }

    public void setOnRewind(bool newOnRewind) {
        _onRewind = newOnRewind;
    }

    public bool getOnRewind() {
        return _onRewind;
    }

    public float getMaxTime() {
        return _maxTime;
    }

    public float getTimeRewindLimit() {
        return _timeRewindLimit;
    }
}

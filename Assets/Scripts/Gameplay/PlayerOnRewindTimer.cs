using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using UnityEngine;
using TMPro;

public class PlayerOnRewindTimer : MonoBehaviour
{
    // Text
    private TextMeshPro _timerTextBox;
   
    // Time Manager object
    TimeManager _timeManager;

    // Player
    PlayerController _playerController;

    // Timer variables
    float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _timerTextBox = GetComponent<TextMeshPro>();
        _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        if (_playerController.timeRewindLimitTeleport && (_timeManager.getMaxTime() - _timeManager.GetCustomTime() > 0.1f || _timeManager.GetCustomTime() > _timeManager.getMaxTime() - _timeManager.getTimeRewindLimit()))
        {
            currentTime = - (_timeManager.getMaxTime() - _timeManager.getTimeRewindLimit() - _timeManager.GetCustomTime());
        }
        else if (_timeManager.GetCustomTime() > _timeManager.getTimeRewindLimit())
        {
            currentTime = _timeManager.getTimeRewindLimit();
            _playerController.timeRewindLimitTeleport = false;
        }
        else
        {
            currentTime = _timeManager.GetCustomTime();
            _playerController.timeRewindLimitTeleport = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        _timerTextBox.text = (currentTime).ToString("0");
        currentTime -= 1 * Time.deltaTime;
    }
}

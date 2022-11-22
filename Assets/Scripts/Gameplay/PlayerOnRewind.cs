using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;

public class PlayerOnRewind : MonoBehaviour
{

    //private List<Vector3> _rewindList = new List<Vector3>();

    private RewindSaveInfo _rewindSaveInfo;

    private Dictionary<float, TimeRewindObject> _rewindList;
    private PlayerController _playerController;
    private Transform _transform;

    private Rigidbody2D _rigidbody2D;

    private TimeManager _timeManager;

    private float result;

    private float currentTimeRewind;
    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        _rewindSaveInfo = GetComponent<RewindSaveInfo>();
        //currentTimeRewind = (float)System.Math.Round(Time.time, 2);
        currentTimeRewind -= currentTimeRewind % 0.02f;
        //Debug.Log(_rewindSaveInfo);
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        Rewind();
    }

    private void Rewind()
    {
        //Debug.Log("Current time on rewind: " + currentTimeRewind);
        //_rewindSaveInfo.GetTimeRewindObject(currentTimeRewind);
        try
        {
            _rigidbody2D.MovePosition(GetTimeRewindObject(_timeManager.GetCustomTime()).GetPosition());
        }
        catch (System.Exception)
        {
            Debug.Log("not found : " + currentTimeRewind);
        }


        currentTimeRewind -= Time.fixedDeltaTime;
        currentTimeRewind = (float) System.Math.Round(currentTimeRewind, 2);
        currentTimeRewind -= currentTimeRewind % 0.02f;

        if (_timeManager.GetCustomTime() < 0.0f) {
            _playerController.StopRewinding();
        }

        /* int l = _rewindList.Count;
         if (l > 0)
         {
             // Debug.Log(l);
             Vector3 to_move = _rewindList[l - 1];
             _rewindList.RemoveAt(l - 1);
             //_rigidbody2D.MovePosition(to_move);
             _transform.position = to_move;

         }
         else
         {
             //_rigidbody2D.MovePosition(_transform.position);
             Destroy(gameObject);
             _playerController.StopRewinding();
         }*/

    }

    public void setRewindSaveInfo(RewindSaveInfo rewindSaveInfo)
    {
        //reset _rewindSaveInfo
        _rewindList = new Dictionary<float, TimeRewindObject>();

        //recreate a new list with all the elements of the list in parameter
        foreach (KeyValuePair<float,TimeRewindObject> element in rewindSaveInfo.GetRewindList())
        {
            _rewindList.Add((float) element.Key, element.Value);
            //Debug.Log("Added: " + element.Key + " | " + element.Value.GetPosition());
        }

        //_rewindSaveInfo = rewindSaveInfo;
        //Debug.Log("set rewind save info");
    }

    public void SetCurrentRewindTime(float new_currentRewindTime) {
        currentTimeRewind = (float) System.Math.Round(new_currentRewindTime, 2);
        currentTimeRewind -= currentTimeRewind % 0.02f;
    }

    public void setTimeManager(TimeManager newTimeManager) {
        //Debug.Log("set the time manager");
        _timeManager = newTimeManager;
        //_rewindSaveInfo._timeManager = _timeManager;
    }


    private TimeRewindObject GetTimeRewindObject(float time)
    {
        //iterate through the list to find the time
        foreach (KeyValuePair<float, TimeRewindObject> element in _rewindList)
        {
            float result = (float) element.Key - (float) time;
            
            if ((float) System.Math.Round(result, 2) == 0.00f)
            {
                return element.Value;
            }
        }
        return null;
    }
    public void setPlayerController(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public float getCurrentTimeRewind()
    {
        return currentTimeRewind;
    }


    public void Destroy()
    {
        Destroy(gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;

public class PlayerOnRewind : MonoBehaviour
{

    //private List<Vector3> _rewindList = new List<Vector3>();
    
    private RewindSaveInfo _rewindSaveInfo;
    private PlayerController _playerController;
    private Transform _transform;

    private float currentTimeRewind;
    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        _rewindSaveInfo = GetComponent<RewindSaveInfo>();
        currentTimeRewind = (float) System.Math.Round(Time.time,2);
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
        Debug.Log("Current time on rewind 1: " + currentTimeRewind);
        try
        {
            Debug.Log(_rewindSaveInfo.GetTimeRewindObjectAccordingToTime(currentTimeRewind).GetTransform().position);
        }
        catch (System.Exception)
        {
        }
        

        currentTimeRewind -= Time.fixedDeltaTime;

        //Debug.Log("Current time on rewind 2: " + currentTimeRewind);


        currentTimeRewind = (float) System.Math.Round(currentTimeRewind,2);
        currentTimeRewind -= currentTimeRewind % 0.02f;
        
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
        _rewindSaveInfo = rewindSaveInfo;
        //Debug.Log(_rewindSaveInfo.GetLength());
    }
    public void setPlayerController(PlayerController playerController)
    {
        _playerController = playerController;
    }


    public void Destroy()
    {
        Destroy(gameObject);
    }
}

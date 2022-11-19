using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RewindSaveInfo : MonoBehaviour
{

    private Dictionary<float, TimeRewindObject> _timeRewindObjects;
    private Transform _transform;

    private void Start()
    {
        _timeRewindObjects = new Dictionary<float, TimeRewindObject>();
        _transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        _transform = GetComponent<Transform>();
        IncrementRewindingList();
    }


    public void IncrementRewindingList()
    {
        try
        {
            AddTimeRewindObject(CreateTimeRewindObject(), (float) System.Math.Round(Time.time,2));
        }
        catch (System.Exception)
        {
           // Debug.Log("Already exists");
        }
        
    }

    public TimeRewindObject CreateTimeRewindObject()
    {
        return new TimeRewindObject(_transform, true);
    }

    public void AddTimeRewindObject(TimeRewindObject timeRewindObject, float time)
    {
        _timeRewindObjects.Add(time, timeRewindObject);
        //Debug.Log("Current time: " + time + " | " + "Current rotation: " + _timeRewindObjects[time].GetTransform().rotation);
    }

    public TimeRewindObject GetTimeRewindObjectAccordingToTime(float time)
    {
        return _timeRewindObjects[time];
    }

    /*
    return length of the _timeRewindObjects
     */
    public int GetLength()
    {
        return _timeRewindObjects.Count;
    }
}

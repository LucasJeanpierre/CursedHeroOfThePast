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
        IncrementRewindingList();
    
    }


    public void IncrementRewindingList()
    {
        AddTimeRewindObject(CreateTimeRewindObject(), (float) System.Math.Round(Time.time,2));
    }

    public TimeRewindObject CreateTimeRewindObject()
    {
        return new TimeRewindObject(_transform, true);
    }

    public void AddTimeRewindObject(TimeRewindObject timeRewindObject, float time)
    {
        _timeRewindObjects.Add(time, timeRewindObject);
        Debug.Log(time);
    }
}

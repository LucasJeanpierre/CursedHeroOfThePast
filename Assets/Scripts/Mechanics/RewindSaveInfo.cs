using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RewindSaveInfo : MonoBehaviour
{

    private Dictionary<float, TimeRewindObject> _timeRewindObjects;
    private Transform _transform;

    private Transform _tmpTransform;

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
      AddTimeRewindObject(CreateTimeRewindObject(), (float) System.Math.Round(Time.time,2));
        
    }

    public TimeRewindObject CreateTimeRewindObject()
    {
        return new TimeRewindObject(
            new Vector3(_transform.position.x, _transform.position.y, _transform.position.z),
            new Quaternion(_transform.rotation.x, _transform.rotation.y, _transform.rotation.z, _transform.rotation.w),
            new Vector3(_transform.localScale.x, _transform.localScale.y, _transform.localScale.z),
            true);
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
    return the list of time rewind objects
    */
    public Dictionary<float, TimeRewindObject> GetRewindList()
    {
        /*foreach (var item in _timeRewindObjects)
        {
            Debug.Log("item for : " + item.Key + " = " + item.Value.GetTransform().position);
        }*/
        return _timeRewindObjects;
    }


    public void RewindTo(float time) {
        _transform.position = _timeRewindObjects[time].GetPosition();
        _transform.rotation = _timeRewindObjects[time].GetRotation();
        _transform.localScale = _timeRewindObjects[time].GetScale();
    }

    /*
    return length of the _timeRewindObjects
     */
    public int GetLength()
    {
        return _timeRewindObjects.Count;
    }
}

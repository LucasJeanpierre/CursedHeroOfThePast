using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RewindSaveInfo : MonoBehaviour
{

    private Dictionary<float, TimeRewindObject> _timeRewindObjects;

    [SerializeField] public TimeManager _timeManager;
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
        if (!_timeManager.getOnRewind()) {
            IncrementRewindingList();
        }   
    }


    public void IncrementRewindingList()
    {
      AddTimeRewindObject(CreateTimeRewindObject(), _timeManager.GetCustomTime());
        
    }

    public TimeRewindObject CreateTimeRewindObject()
    {
        return new TimeRewindObject(
            new Vector3(_transform.position.x, _transform.position.y, _transform.position.z),   
            new Vector3(_transform.eulerAngles.x, _transform.eulerAngles.y, _transform.eulerAngles.z),
            new Vector3(_transform.localScale.x, _transform.localScale.y, _transform.localScale.z),
            true);
    }

    public void AddTimeRewindObject(TimeRewindObject timeRewindObject, float time)
    {
        try
        {
            _timeRewindObjects.Add(time, timeRewindObject);
        }
        catch (System.Exception)
        {
            _timeRewindObjects[time] = timeRewindObject;
        }
        
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
        Debug.Log("Rewind to: " + time);
        Debug.Log("Rotation: " + this.GetTimeRewindObject(time).GetRotation());

        _transform.position = this.GetTimeRewindObject(time).GetPosition();
        _transform.rotation = this.GetTimeRewindObject(time).GetRotation();
        _transform.localScale = this.GetTimeRewindObject(time).GetScale();
    }

     private TimeRewindObject GetTimeRewindObject(float time)
    {
        //iterate through the list to find the time
        foreach (KeyValuePair<float, TimeRewindObject> element in _timeRewindObjects)
        {
            float result = (float) element.Key - (float) time;
            
            if ((float) System.Math.Round(result, 2) == 0.00f)
            {
                return element.Value;
            }
        }
        return null;
    }



    /*
    return length of the _timeRewindObjects
    */
    public int GetLength()
    {
        return _timeRewindObjects.Count;
    }
}

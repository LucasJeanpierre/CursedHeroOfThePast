using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewindObject
{

    private Vector3 _position;
    private Vector3 _rotation;
    private Vector3 _scale;

    private bool _available;

    public TimeRewindObject(Vector3 position, Vector3 rotation, Vector3 scale, bool available)
    {
        _position = position;
        _rotation = rotation;
        _scale = scale;
        _available = available;
    }

    public bool GetAvailable()
    {
        return _available;
    }

    public void SetAvailable(bool available)
    {
        _available = available;
    }

    public Vector3 GetPosition()
    {
        return _position;
    }

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(_rotation);
    }

    public Vector3 GetScale()
    {
        return _scale;
    }

}

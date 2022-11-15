using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewindObject
{

    private Transform _transform;
    private bool _available;

    public TimeRewindObject(Transform transform, bool available)
    {
        _transform = transform;
    }

    public bool GetAvailable()
    {
        return _available;
    }

    public Transform GetTransform()
    {
        return _transform;
    }

}

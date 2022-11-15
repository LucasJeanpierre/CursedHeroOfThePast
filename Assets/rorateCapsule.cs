using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rorateCapsule : MonoBehaviour
{

    private Transform _transform;
    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        var rotationVector = _transform.rotation.eulerAngles;
        rotationVector.z += (float) 50 * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rotationVector);
    }
}

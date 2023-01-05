using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using Platformer.Model;
using Platformer.Gameplay;
using UnityEngine;

public class LaserShootingVFX : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private Transform _transform;
    Laser _laser;

    private bool timeToLaunch = false;
    public float _scaleUpSpeed = 0.0025f;
    private float _delayBefore = 0.5f;
    public float _endWidth = 0.2f;
    private float _width = 0f;

    void Start()
    {
        _transform = this.transform;
        _laser = GameObject.Find("Laser Gun").GetComponent<Laser>();
        InvokeRepeating("LaunchVFX", _laser.startAfter - _delayBefore, _laser.shootOnTime + _laser.shootOffTime);
        _delayBefore = _laser.shootOffTime / 2;
        lineRenderer.SetWidth(_width, _width);
    }

    void LaunchVFX()
    {
        timeToLaunch = true;
    }

    void Update()
    {
        // Create a ray going down to check colision
        RaycastHit2D hit = Physics2D.Raycast((Vector2)_laser.firePoint.position, transform.right);

        if (hit && timeToLaunch)
        {
            // Set start of laser in the gun pointer
            lineRenderer.SetPosition(0, (Vector2)_laser.firePoint.position);
            // Set end of laser in the raycast hit position
            lineRenderer.SetPosition(1, hit.point);

            // Update VFX size
            lineRenderer.SetWidth(_width, _width);
            _width += _scaleUpSpeed;
            if (_width >= _endWidth)
            {
                lineRenderer.SetWidth(0, 0);
                timeToLaunch = false;
                _width = 0;
            }
                
            //_transform.localScale -= new Vector3(_scaleDownSpeed, _scaleDownSpeed, 0);
            //if (_transform.localScale.x <= 0)
            //    timeToLaunch = false;


            //firstHit = false;
        }


    }
}

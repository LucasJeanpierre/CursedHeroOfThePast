using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{
    public class Laser : MonoBehaviour
    {
        // Laser variables
        public Camera cam;
        public LineRenderer lineRendererLaser;
        public LineRenderer lineRendererCharge;
        public Transform firePoint;

        // Effects
        public GameObject StartVFX;
        public GameObject EndVFX;

        // Shooting propriety
        public float startAfter = 1.0f;
        public float shootOnTime = 1.0f;
        public float shootOffTime = 1.0f;

        // Rotating propriety
        public bool rotateLaser = false;
        public float rotateSpeed = 0.5f;

        private bool _laserShooting = false;

        // VFX
        private List<ParticleSystem> particles = new List<ParticleSystem>();
        private bool _timeToLaunch = false;
        public float _scaleUpSpeedCharge = 0.0025f;
        private float _delayBeforeCharge = 0.5f;
        public float _endWidthCharge = 0.2f;
        private float _widthCharge = 0f;

        void Start()
        {
            FillLists();
            DisableLaser();
            InvokeRepeating("LaunchLaser", startAfter, shootOnTime + shootOffTime);
            InvokeRepeating("OffLaser", startAfter + shootOnTime, shootOnTime + shootOffTime);
            // VFX
            InvokeRepeating("LaunchVFX", startAfter - _delayBeforeCharge, shootOnTime + shootOffTime);
            _delayBeforeCharge = shootOffTime / 2;
            lineRendererCharge.SetWidth(_widthCharge, _widthCharge);
        }



        // Update is called once per frame
        void Update()
        {
            if (_laserShooting)
                UpdateLaser();
            if (_timeToLaunch)
                UpdateCharge();
            if (rotateLaser)
            {
                Vector3 rotationToAdd = new Vector3(0, 0, rotateSpeed * Time.deltaTime);
                transform.Rotate(rotationToAdd);
            }
        }

        void EnableLaser()
        {
            // Enable beam
            lineRendererLaser.enabled = true;

            // Enable particles
            for (int i = 0; i < particles.Count; i++)
                particles[i].Play();
        }

        void LaunchLaser()
        {
            if (_laserShooting == false)
            {
                EnableLaser();
                _laserShooting = true;
            }
        }

        void LaunchVFX()
        {
            _timeToLaunch = true;
        }

        void OffLaser()
        {
            if (_laserShooting)
            {
                DisableLaser();
                _laserShooting = false;
            }
        }

        void UpdateCharge()
        {
            RaycastHit2D hit = Physics2D.Raycast((Vector2)firePoint.position, transform.right);

            if (hit)
            {
                // Set start of laser in the gun pointer
                lineRendererCharge.SetPosition(0, (Vector2)firePoint.position);
                // Set end of laser in the raycast hit position
                lineRendererCharge.SetPosition(1, hit.point);

                // Update VFX size
                lineRendererCharge.SetWidth(_widthCharge, _widthCharge);
                _widthCharge += _scaleUpSpeedCharge;
                if (_widthCharge >= _endWidthCharge)
                {
                    lineRendererCharge.SetWidth(0, 0);
                    _timeToLaunch = false;
                    _widthCharge = 0;
                }

            }
        }

        void UpdateLaser()
        {
            // Create a ray going down to check colision
            RaycastHit2D hit = Physics2D.Raycast((Vector2)firePoint.position, transform.right);

            if (hit)
            {
                // Set start of laser in the gun pointer
                lineRendererLaser.SetPosition(0, (Vector2)firePoint.position);
                StartVFX.transform.position = (Vector2)firePoint.position;
                // Set end of laser in the raycast hit position
                lineRendererLaser.SetPosition(1, hit.point);
                EndVFX.transform.position = (Vector2)lineRendererLaser.GetPosition(1);
                // If laser hits player
                if (hit.transform.name == "Player")
                    Schedule<PlayerDeath>();
            }
            else
            {
                DisableLaser();
                _laserShooting = false;
            }

        }

        void DisableLaser()
        {
            // Reset laser line
            lineRendererLaser.SetPosition(0, new Vector2(0, 0));
            lineRendererLaser.SetPosition(1, new Vector2(0, 0));

            // Disable beam
            lineRendererLaser.enabled = false;

            // Disable particles
            for (int i = 0; i < particles.Count; i++)
                particles[i].Stop();
        }


        void FillLists()
        {
            for (int i = 0; i < StartVFX.transform.childCount; i++)
            {
                var ps = StartVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
                if (ps != null)
                    particles.Add(ps);
            }

            for (int i = 0; i < EndVFX.transform.childCount; i++)
            {
                var ps = EndVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
                if (ps != null)
                    particles.Add(ps);
            }
        }
    }
}
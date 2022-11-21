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
        public LineRenderer lineRenderer;
        public Transform firePoint;

        // Effects
        public GameObject StartVFX;
        public GameObject EndVFX;

        // Rotating propriety
        public bool rotateLaser = false;
        public float rotateSpeed = 0.5f;

        private Quaternion rotation;
        private bool _laserShooting = false;

        private List<ParticleSystem> particles = new List<ParticleSystem>();


        void Start()
        {
            FillLists();
            DisableLaser();
            InvokeRepeating("LaunchLaser", 1.0f, 1.0f);
        }



        // Update is called once per frame
        void Update()
        {
            if (_laserShooting)
                UpdateLaser();
            if (rotateLaser)
            {
                Vector3 rotationToAdd = new Vector3(0, 0, rotateSpeed);
                transform.Rotate(rotationToAdd);
            }
        }

        void EnableLaser()
        {
            // Enable beam
            lineRenderer.enabled = true;

            // Enable particles
            for (int i = 0; i < particles.Count; i++)
                particles[i].Play();
        }

        void LaunchLaser()
        {
            if (_laserShooting)
            {
                DisableLaser();
                _laserShooting = false;
            }
            else
            {
                EnableLaser();
                _laserShooting = true;
            }
        }

        void UpdateLaser()
        {
            // Create a ray going down to check colision
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up);
            
            //Debug.Log("Forward gun: " + transform.right);
            //Debug.Log("Forward pointer: " + firePoint.forward);

            if (hit)
            {
                // Set start of laser in the gun pointer
                lineRenderer.SetPosition(0, (Vector2)firePoint.position);
                StartVFX.transform.position = (Vector2)firePoint.position;
                // Set end of laser in the raycast hit position
                lineRenderer.SetPosition(1, hit.point);
                EndVFX.transform.position = (Vector2)lineRenderer.GetPosition(1);
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
            lineRenderer.SetPosition(0, new Vector2(0, 0));
            lineRenderer.SetPosition(1, new Vector2(0, 0));

            // Disable beam
            lineRenderer.enabled = false;

            // Disable particles
            for (int i = 0; i < particles.Count; i++)
                particles[i].Stop();
        }

        void RotateToMouse()
        {
            Vector2 direction = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rotation.eulerAngles = new Vector3(0, 0, angle);
            transform.rotation = rotation;
        }

        void FillLists()
        {
            for(int i = 0; i < StartVFX.transform.childCount; i++)
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
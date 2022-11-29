using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{
    public class LightLaser : MonoBehaviour
    {
        // Laser variables
        public Camera cam;
        public LineRenderer lineRenderer;
        public Transform firePoint;

        // Effects
        public GameObject StartVFX;
        public GameObject EndVFX;

        // Shooting propriety
        public float startAfter = 1.0f;
        public float shootRate = 1.0f;

        // Rotating propriety
        private float spawnRotation;
        public bool rotateLaser = false;
        public float rotateSpeed = 0.5f;

        private Quaternion rotation;
        private bool _laserShooting = true;

        private List<ParticleSystem> particles = new List<ParticleSystem>();

        public Slider _slider;

        private Button _LastPressedButton;

        //[SerializeField] private Button _targetButton;

        void Start()
        {
            FillLists();
            EnableLaser();
            spawnRotation = transform.localEulerAngles.z;
            //print(spawnRotation+"spawnRotation");
            //InvokeRepeating("LaunchLaser", startAfter, shootRate);
        }



        // Update is called once per frame
        void Update()
        {

            if (_laserShooting) UpdateLaser();

            transform.rotation = Quaternion.Euler(0, 0, _slider.getRotation() + spawnRotation);
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

        void UpdateLaser()
        {
            // Create a ray going down to check colision
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);

            //Debug.Log("Forward gun: " + transform.right);
            //Debug.Log("Forward pointer: " + firePoint.forward);

            if (hit)
            {
                EnableLaser();
                
                // Set start of laser in the gun pointer
                lineRenderer.SetPosition(0, (Vector2)firePoint.position);
                StartVFX.transform.position = (Vector2)firePoint.position;
                // Set end of laser in the raycast hit position
                lineRenderer.SetPosition(1, hit.point);
                EndVFX.transform.position = (Vector2)lineRenderer.GetPosition(1);

                if (hit.collider.gameObject.tag == "LightTarget")
                {
                    Debug.Log("Hit target");
                    _LastPressedButton = hit.collider.gameObject.GetComponent<Button>();
                    hit.collider.gameObject.GetComponent<LightTarget>().PressButton();
                } else {
                    if (_LastPressedButton != null) {
                        _LastPressedButton.unPressButton();
                        _LastPressedButton = null;
                    }
                }
            }
            else
            {
                DisableLaser();
                //_laserShooting = false;
            }

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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using Cinemachine;

namespace Platformer.Mechanics
{
    public class PlayerOnRewind : KinematicObject
    {

        //private List<Vector3> _rewindList = new List<Vector3>();

        private RewindSaveInfo _rewindSaveInfo;

        private Dictionary<float, TimeRewindObject> _rewindList;
        private PlayerController _playerController;
        private Transform _transform;
        internal Animator animator;
        public Collider2D collider2d;

        private Rigidbody2D _rigidbody2D;

        private TimeManager _timeManager;

        private float result;

        private float currentTimeRewind;

        private Vector2 newPos;

        private bool _isStatic;

        // Player proprieties
        public float maxSpeed = 7;
        public float jumpTakeOffSpeed = 7;
        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        private bool isDashing;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            _transform = this.transform;
        }

        // Start is called before the first frame update
        protected new void Start()
        {
            _transform = this.transform;
            _rigidbody2D = this.GetComponent<Rigidbody2D>();
            _rewindSaveInfo = GetComponent<RewindSaveInfo>();
            //currentTimeRewind = (float)System.Math.Round(Time.time, 2);
            currentTimeRewind -= currentTimeRewind % 0.02f;
            //Debug.Log(_rewindSaveInfo);
        }

        
        protected override void FixedUpdate()
        {
            // Clone rewind animation
            if (jumpState == JumpState.Grounded && (velocity.y > 0.2f || velocity.y < -0.2f))
                jumpState = JumpState.PrepareToJump;
            else if (IsGrounded)
                stopJump = true;
            UpdateJumpState();
            base.Update();
            base.FixedUpdate();

            // Rewind
            if (!_isStatic)
                Rewind();
        }
        

        private void Rewind()
        {
            //Debug.Log("Current time on rewind: " + currentTimeRewind);
            //_rewindSaveInfo.GetTimeRewindObject(currentTimeRewind);
            try
            {
                newPos = GetTimeRewindObject(_timeManager.GetCustomTime()).GetPosition();
                move.x = newPos.x - transform.position.x;
                move.y = newPos.y - transform.position.y;
                velocity.x = (float) move.x / Time.deltaTime;
                velocity.y = (float) move.y / Time.deltaTime;
                _rigidbody2D.MovePosition(newPos);
            }
            catch (System.Exception)
            {
                Debug.Log("not found : " + currentTimeRewind);
            }


            currentTimeRewind -= Time.fixedDeltaTime;
            currentTimeRewind = (float)System.Math.Round(currentTimeRewind, 2);
            currentTimeRewind -= currentTimeRewind % 0.02f;

            if (_timeManager.GetCustomTime() < 0.0f)
            {
                _playerController.StopRewinding();
            }

        }

        public void setRewindSaveInfo(RewindSaveInfo rewindSaveInfo)
        {
            //reset _rewindSaveInfo
            _rewindList = new Dictionary<float, TimeRewindObject>();

            //recreate a new list with all the elements of the list in parameter
            foreach (KeyValuePair<float, TimeRewindObject> element in rewindSaveInfo.GetRewindList())
            {
                _rewindList.Add((float)element.Key, element.Value);
                //Debug.Log("Added: " + element.Key + " | " + element.Value.GetPosition());
            }

            //_rewindSaveInfo = rewindSaveInfo;
            //Debug.Log("set rewind save info");
        }

        public void SetCurrentRewindTime(float new_currentRewindTime)
        {
            currentTimeRewind = (float)System.Math.Round(new_currentRewindTime, 2);
            currentTimeRewind -= currentTimeRewind % 0.02f;
        }

        public void setTimeManager(TimeManager newTimeManager)
        {
            //Debug.Log("set the time manager");
            _timeManager = newTimeManager;
            //_rewindSaveInfo._timeManager = _timeManager;
        }

        private TimeRewindObject GetTimeRewindObject(float time)
        {
            //iterate through the list to find the time
            foreach (KeyValuePair<float, TimeRewindObject> element in _rewindList)
            {
                float result = (float)element.Key - (float)time;

                if ((float)System.Math.Round(result, 2) == 0.00f)
                {
                    return element.Value;
                }
            }
            return null;
        }
        public void setPlayerController(PlayerController playerController)
        {
            _playerController = playerController;
        }

        public float getCurrentTimeRewind()
        {
            return currentTimeRewind;
        }

        public void setIsStatic(bool isStatic)
        {
            _isStatic = isStatic;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        // Clone animation on rewind
        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    Debug.Log("AKI 1");
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Debug.Log("AKI 2");
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Debug.Log("AKI 3");
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    Debug.Log("AKI 4");
                    break;
            }
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }

        protected override void ComputeVelocity()
        {
            if (isDashing)
            {
                velocity.y = 0;
                return;
            }
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }


            if (move.x > 0.01f)
                spriteRenderer.flipX = true;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = false;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }
    }
}
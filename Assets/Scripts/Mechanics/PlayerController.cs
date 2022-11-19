using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using System;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        private List<Vector3> _rewindList = new List<Vector3>();
        private Boolean _onRewind = false;

        [SerializeField] private PlayerOnRewind _staticPlayerOnRewind;
        private Vector3 _lastPositionBeforeRewind;
        private Transform _transform;
        private Rigidbody2D _rigidbody2D;

        private PlayerOnRewind staticPlayerOnRewind;

        private float _facingDirection = 1f;
        private bool canDash = true;
        private bool isDashing;
        public float dashingPower = 2f;
        private float dashingTime = 0.2f;
        private float dashingCooldown = 1f;

        [SerializeField] private TrailRenderer tr;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _transform = this.transform;
        }

        protected override void Update()
        {   
            Debug.Log(velocity.y);
            _facingDirection = Input.GetAxis("Horizontal")!=0 ? Input.GetAxis("Horizontal") : _facingDirection;

            if(isDashing)
            {
                return; 
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartCoroutine("Dash");
            }

            if (Input.GetKeyDown(KeyCode.Backspace) && (_onRewind == true))
            {
                StopRewinding();
            } else if (Input.GetKeyDown(KeyCode.Backspace) && (_onRewind == false))
            {
                StartRewinding();
            }

            if (_onRewind)
            {
                //Rewind();
            }
            else
            {
                Vector2 current_pos = new Vector2(_transform.position.x, _transform.position.y);
                Vector2 new_pos = Vector2.zero;
                new_pos.x = transform.position.x;
                new_pos.y = transform.position.y;
                _lastPositionBeforeRewind = new_pos;
                // if (current_pos!=new_pos){
                //     _rewindList.Add(new_pos);
                // }
                _rewindList.Add(new_pos);


                //_rigidbody2D.MovePosition(new_pos);
            }

            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();
        }

        public void StartRewinding()
        {
            _onRewind = true;
            staticPlayerOnRewind = Instantiate(_staticPlayerOnRewind, _lastPositionBeforeRewind, _transform.rotation);
            staticPlayerOnRewind.setRewindList(_rewindList);
            staticPlayerOnRewind.setPlayerController(this);
            //controlEnabled = false;

        }

        public void StopRewinding()
        {
            _onRewind = false;
            if (staticPlayerOnRewind != null)
            {
                _transform.position = staticPlayerOnRewind.transform.position;
                Destroy(staticPlayerOnRewind.gameObject);
            }
            //_rewindList = new List<Vector3>();
            //staticPlayerOnRewind.setOnRewind(false);
            //controlEnabled = true;
            //staticPlayerOnRewind.Destroy();
        }

        private void Rewind()
        {


            int l = _rewindList.Count;
            if (l > 0)
            {
                // Debug.Log(l);
                Vector3 to_move = _rewindList[l - 1];
                _rewindList.RemoveAt(l - 1);
                //_rigidbody2D.MovePosition(to_move);
                _transform.position = to_move;

            }
            else
            {
                //_rigidbody2D.MovePosition(_transform.position);
            }

        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
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
            }else if(isDashing){
                velocity.y=0;
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }


        private IEnumerator Dash(){
            float dashAmount = dashingPower * Mathf.Sign(_facingDirection);
            canDash = false;
            isDashing = true;
            Debug.Log("Stop gravity");
            _rigidbody2D.velocity += new Vector2(dashAmount, 0);
            tr.emitting = true;
            yield return new WaitForSeconds(dashingTime);
            tr.emitting = false;
            _rigidbody2D.velocity -= new Vector2(dashAmount, 0);
            Debug.Log("Start gravity");
            isDashing = false;
            yield return new WaitForSeconds(dashingCooldown);
            canDash = true;

        }
    }
}
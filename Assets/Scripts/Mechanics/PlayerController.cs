using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using System;
using Cinemachine;
using UnityEngine.InputSystem;

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


        public PlayerControls _playerControls;

        private InputAction jumpInputAction;
        private InputAction dashInputAction;
        private InputAction moveInputAction;
        private InputAction rewindInputAction;
        private InputAction createCloneInputAction;

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
        /*internal new*/
        public Collider2D collider2d;
        /*internal new*/
        public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        public int maxNumberOfClones = 1;

        private int _numberOfClones = 0;


        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        //private List<Vector3> _rewindList = new List<Vector3>();

        private RewindSaveInfo rewindSaveInfo;

        private Boolean _onRewind = false;

        [SerializeField] private PlayerOnRewind __playerOnRewind;
        private Vector3 _lastPositionBeforeRewind;
        private Transform _transform;
        private Rigidbody2D _rigidbody2D;

        private PlayerOnRewind _playerOnRewind;

        [SerializeField] public TimeManager _timeManager;


        private float _facingDirection = 1f;
        private bool canDash = true;
        private bool isDashing;
        public float dashingPower = 2f;
        private float dashingTime = 0.2f;
        private float dashingCooldown = 1f;
        private float _jumppress; 


        private IEnumerator coroutine;

        //list of clones
        private List<PlayerOnRewind> _clones = new List<PlayerOnRewind>();

        [SerializeField] private TrailRenderer tr;


        [SerializeField] private CinemachineTargetGroup _cinemachineTargetGroup;


        [SerializeField] private float delayBetweenClones = 0.05f;

        //private GravityModifier _gravityModifier;

        public bool timeRewindLimitTeleport = false;

        void OnEnable()
        {
            base.OnEnable();
            _playerControls.Player.Enable();
        }

         void OnDisable()
        {
            base.OnDisable();
            _playerControls.Player.Disable();
        }

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            //_gravityModifier = GetComponent<gra
            _transform = this.transform;
            rewindSaveInfo = GetComponent<RewindSaveInfo>();
             _playerControls = new PlayerControls();

            dashInputAction = _playerControls.Player.Dash;
            dashInputAction.performed += OnDashInput;

            jumpInputAction = _playerControls.Player.Jump;
            jumpInputAction.performed += OnJumpInput;
            jumpInputAction.canceled += OnJumpInput;

            rewindInputAction = _playerControls.Player.Rewind;
            rewindInputAction.performed += OnRewindInput;

            createCloneInputAction = _playerControls.Player.CreateClone;
            createCloneInputAction.performed += OnCreateCloneInput;

            moveInputAction = _playerControls.Player.Move;
            moveInputAction.performed += OnMoveInput;
            moveInputAction.canceled += OnMoveInput;
        }

        private void OnMoveInput(InputAction.CallbackContext context)
        {
            move = context.ReadValue<Vector2>();
        }

        private void OnJumpInput(InputAction.CallbackContext context)
        {
            _jumppress = context.ReadValue<float>();
        }

        private void OnDashInput(InputAction.CallbackContext context)
        {
            if (canDash)
            {
                StartCoroutine("Dash");
            }
        }

        private void OnRewindInput(InputAction.CallbackContext context)
        {
            if (_onRewind == true)
            {
                StopRewinding();
            }
            else
            {
                StartRewinding();
            }
        }

        private void OnCreateCloneInput(InputAction.CallbackContext context)
        {
            if(_onRewind == true)
                CreateClone();
        }

        protected override void Update()
        {
            _facingDirection = move.x != 0 ? move.x : _facingDirection;

            if (isDashing)
            {
                return;
            }


            else
            {
                Vector2 current_pos = new Vector2(_transform.position.x, _transform.position.y);
                Vector2 new_pos = Vector2.zero;
                new_pos.x = transform.position.x;
                new_pos.y = transform.position.y;
                _lastPositionBeforeRewind = new_pos;
                //rewindSaveInfo.IncrementRewindingList();
                // if (current_pos!=new_pos){
                //     _rewindList.Add(new_pos);
                // }
                //_rewindList.Add(new_pos);


                //_rigidbody2D.MovePosition(new_pos);
            }

            if (controlEnabled)
            {
                //move.x = Input.GetAxis("Horizontal");
                if (jumpState == JumpState.Grounded && _jumppress == 1)
                    jumpState = JumpState.PrepareToJump;
                else if (_jumppress == 0)
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
            _timeManager.setOnRewind(true);
            _onRewind = true;
            _playerOnRewind = Instantiate(__playerOnRewind, _transform.position, _transform.rotation);
            _playerOnRewind.setRewindSaveInfo(rewindSaveInfo);
            _playerOnRewind.setTimeManager(_timeManager);
            Debug.Log(_playerOnRewind._timeManager);
            //_playerOnRewind.SetCurrentRewindTime(_timeManager.GetCustomTime());
            _playerOnRewind.setPlayerController(this);

            _cinemachineTargetGroup.AddMember(_playerOnRewind.transform, 1f, 0f);
            //controlEnabled = false;

        }

        public void CreateClone()
        {
            if (_clones.Count >= maxNumberOfClones)
            {
                return;
            }
            PlayerOnRewind clone = Instantiate(__playerOnRewind, _playerOnRewind.transform.position, _playerOnRewind.transform.rotation);
            //clone.setRewindSaveInfo(rewindSaveInfo);
            //clone.setTimeManager(_timeManager);
            //clone.setPlayerController(this);
            clone.setIsStatic(true);

            _clones.Add(clone);
            _numberOfClones++;
        }

        public void StopRewinding()
        {
            // if (staticPlayerOnRewind != null)
            // {
            //     _transform.position = staticPlayerOnRewind.transform.position;
            //     _timeManager.RewindAllAffectedObjects(staticPlayerOnRewind.getCurrentTimeRewind());
            //     _cinemachineTargetGroup.RemoveMember(staticPlayerOnRewind.transform);
            //     Destroy(staticPlayerOnRewind.gameObject);
            // }
            //_rewindList = new List<Vector3>();
            //staticPlayerOnRewind.setOnRewind(false);
            //controlEnabled = true;
            //staticPlayerOnRewind.Destroy();



            //go trough the list of clones and destroy them
            //Debug.Log(gameObject.name);
            //Debug.Log(_playerOnRewind.gameObject.name);
            coroutine = TeleportToAllClonesPositions(delayBetweenClones);
            StartCoroutine(coroutine);

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


        private IEnumerator Dash()
        {
            float dashAmount = dashingPower * Mathf.Sign(_facingDirection);
            canDash = false;
            isDashing = true;

            gravityModifier = 0f;
            //_rigidbody2D.velocity += new Vector2(dashAmount, 0);
            base.velocity.y = 0;
            PerformMovement(new Vector2(dashAmount, 0), false);
            tr.emitting = true;
            yield return new WaitForSeconds(dashingTime);
            tr.emitting = false;
            //_rigidbody2D.velocity -= new Vector2(dashAmount, 0);

            gravityModifier = 1f;
            isDashing = false;
            yield return new WaitForSeconds(dashingCooldown);
            canDash = true;

        }


        IEnumerator TeleportToAllClonesPositions(float delay)
        {
            foreach (PlayerOnRewind clone in _clones)
            {
                _transform.position = clone.transform.position;
                Destroy(clone.gameObject);
                yield return new WaitForSeconds(delay);
            }

            _clones = new List<PlayerOnRewind>();


            if (_playerOnRewind != null)
            {
                _transform.position = _playerOnRewind.transform.position;
                _timeManager.RewindAllAffectedObjects();
                _cinemachineTargetGroup.RemoveMember(_playerOnRewind.transform);
                Destroy(_playerOnRewind.gameObject);
            }


            _onRewind = false;
            _timeManager.setOnRewind(false);

        }
    }
}
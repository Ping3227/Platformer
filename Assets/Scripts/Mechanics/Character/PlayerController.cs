using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
namespace Platformer.Mechanics
{
    /// <summary>
    /// player Controller than controll all physic and animation of player
    /// </summary>
    [RequireComponent(typeof(Stamina), typeof(Health), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {

        private Stamina stamina;
        private Health health;
        private Animator animator;
        void OnEnable()
        {
            stamina = GetComponent<Stamina>();
            health = GetComponent<Health>();
            animator = GetComponent<Animator>();
        }
        /// <summary>
        /// Receive input 
        /// </summary>

        void Update()
        {
            Input = new InputCollection {
                JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = UnityEngine.Input.GetButtonUp("Jump"),
                X = UnityEngine.Input.GetAxisRaw("Horizontal")
            };
            if (Input.JumpDown)
            {
                _TimePressedJump = Time.time;
            }

        }
        private InputCollection Input;
        #region Input
        public struct InputCollection
        {
            public float X;
            public bool JumpDown;
            public bool JumpUp;
        }
        #endregion
        void FixedUpdate()
        {
            CalculateCollision();
            CalculateMove();
            CalculateJump();
            Move();
            // UpdateAnimation(); 
        }
        private float _CurrentVerticalSpeed;
        private float _CurrentHorizontalSpeed;
        #region Collision

        [Header("Collision")]
        private bool _CollisionUP, _CollisionRight, _CollisionDown, _CollisionLeft;
        [SerializeField] private Bounds _CharacterBounds;
        [SerializeField] private LayerMask _GroundLayer;
        [SerializeField] private float _DetectionRayLength = 0.1f;
        [SerializeField] private int _DetectorCount = 3;
        [SerializeField][Range(0.1f, 0.3f)] private float _RayBuffer = 0.1f;
        void CalculateCollision() {

            var b = new Bounds(transform.position + _CharacterBounds.center, _CharacterBounds.size);

            var NewCollisionDown = RunDetection(new Vector2(b.min.x + _RayBuffer, b.min.y), new Vector2(b.max.x - _RayBuffer, b.min.y), Vector2.down);

            if (_CollisionDown && !NewCollisionDown) _TimeLeftGround = Time.time; // Only trigger when first leaving
            else if (!_CollisionDown && NewCollisionDown)
            {
                _CoyoteUsable = true;
            }

            _CollisionDown = NewCollisionDown;
            _CollisionUP = RunDetection(new Vector2(b.min.x + _RayBuffer, b.max.y), new Vector2(b.max.x - _RayBuffer, b.max.y), Vector2.up);
            _CollisionLeft = RunDetection(new Vector2(b.min.x, b.min.y + _RayBuffer), new Vector2(b.min.x, b.max.y - _RayBuffer), Vector2.left);
            _CollisionRight = RunDetection(new Vector2(b.max.x, b.min.y + _RayBuffer), new Vector2(b.max.x, b.max.y - _RayBuffer), Vector2.right);



            bool RunDetection(Vector2 start, Vector2 end, Vector2 Direction)
            {
                return EvaluateRayPositions(start, end).Any(point => Physics2D.Raycast(point, Direction, _DetectionRayLength, _GroundLayer));
            }
        }
        private IEnumerable<Vector2> EvaluateRayPositions(Vector2 start, Vector2 end)
        {
            for (var i = 0; i < _DetectorCount; i++)
            {
                var t = (float)i / (_DetectorCount - 1);
                yield return Vector2.Lerp(start, end, t);
            }
        }
        #endregion

        #region jump

        [Header("Jump")]
        [SerializeField] float JumpAllowTime;
        private float _TimePressedJump;
        private bool _JumpBuffer;
        [SerializeField] float CoyoteAllowTime;

        private bool _CoyoteUsable;
        private bool CanUseCoyote => _CoyoteUsable && !_CollisionDown && Time.time - _TimeLeftGround < CoyoteAllowTime;
        private float _TimeLeftGround;
        bool EndJumpEarly;
        [SerializeField] float JumpDecrease;
        [SerializeField] float JumpSpeed;
        [SerializeField] float FallSpeed;
        [SerializeField] float FallDecrease;
        [SerializeField] float MaxFallSpeed;
        private bool _EndJumpEarly;
        void CalculateJump() {
            if (_CollisionDown) // ON GROUND
            {
                if (_JumpBuffer && !_CollisionUP) { //Jump
                    _CurrentVerticalSpeed = JumpSpeed;
                    _EndJumpEarly = false;
                    _CoyoteUsable = false;
                    _JumpBuffer = false;
                }
                else { //Not Jump
                    _CurrentVerticalSpeed = 0;
                    _CoyoteUsable = true;
                }
            }
            else { // IN AIR
                if (_CurrentVerticalSpeed > 0) // Already Jumping 
                {
                    if (Input.JumpUp && !_EndJumpEarly)
                    {
                        _EndJumpEarly = true;
                    }
                    if (_EndJumpEarly) _CurrentVerticalSpeed = FallSpeed;
                    else _CurrentVerticalSpeed -= JumpDecrease;

                    if (_CollisionUP) _CurrentVerticalSpeed = 0;
                }
                else { //Falling 
                    if (CanUseCoyote && (Input.JumpDown || _JumpBuffer)) //Jump
                    {
                        _CurrentVerticalSpeed = JumpSpeed;
                        _EndJumpEarly = false;
                        _CoyoteUsable = false;
                        _JumpBuffer = false;
                    }
                    else {
                        _CurrentVerticalSpeed = Mathf.Max(_CurrentVerticalSpeed - FallDecrease, MaxFallSpeed);
                    }

                }
            }
            // Add slide down wall on IN AIR
        }
        #endregion

        #region move
        [Header("Move")]
        [SerializeField] float MoveSpeed;
        void CalculateMove() {
            // Apex 
            // Move 
            if (!_CollisionRight && Input.X > 0)
            {
                _CurrentHorizontalSpeed = MoveSpeed;
            }
            else if (!_CollisionLeft && Input.X < 0)
            {
                _CurrentHorizontalSpeed = -MoveSpeed;
            }
            else {
                _CurrentHorizontalSpeed = 0;
            }

        }
        #endregion
        #region Animation
        [Header("Animation")]
        private bool IsJumping;
        private bool IsLanding;
        void UpdateAnimation() {
            animator.SetBool("IsJumping", IsJumping);
            animator.SetBool("IsLanding", IsLanding);
            animator.SetFloat("HorizontalSpeed", _CurrentHorizontalSpeed);
            animator.SetFloat("VerticalSpeed", _CurrentVerticalSpeed);
        }
        #endregion
        #region Move 
        [Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
        [SerializeField] int _ColliderIterations = 10;
        void Move() {
            var pos = transform.position + _CharacterBounds.center;
            var move = new Vector3(_CurrentHorizontalSpeed, _CurrentVerticalSpeed) * Time.deltaTime; ; 
            var furthestPoint = pos + move;

            // might have bug whild low fps
            var hit = Physics2D.OverlapBox(furthestPoint, _CharacterBounds.size, 0, _GroundLayer);
            if (!hit)
            {
                transform.position += move;
                return;
            }

            // otherwise increment away from current pos; see what closest position we can move to
            var InitialPosition = transform.position;
            for (int i = 1; i < _ColliderIterations; i++)
            {
                // increment to check all but furthestPoint - we did that already
                var t = (float)i / _ColliderIterations;
                var posToTry = Vector2.Lerp(pos, furthestPoint, t);

                if (Physics2D.OverlapBox(posToTry, _CharacterBounds.size, 0, _GroundLayer))
                {
                    transform.position = InitialPosition;

                    // We've landed on a corner or hit our head on a ledge. Nudge the player gently
                    if (i == 1)
                    {
                        if (_CurrentVerticalSpeed < 0) _CurrentVerticalSpeed = 0;
                        var dir = transform.position - hit.transform.position;
                        transform.position += dir.normalized * move.magnitude;
                    }

                    return;
                }

                InitialPosition = posToTry;
            }
        }
        #endregion
    }


}
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.Timeline;
using UnityEditor.Build;
using Cinemachine;

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
        private Rigidbody2D rigbody;
        public CinemachineVirtualCamera CurrentCamera;


        void OnEnable()
        {
            stamina = GetComponent<Stamina>();
            health = GetComponent<Health>();
            animator = GetComponent<Animator>();
            rigbody = GetComponent<Rigidbody2D>();
        }
        /// <summary>
        /// Receive input 
        /// </summary>
  
        void Update()
        {
            Input.JumpDown = UnityEngine.Input.GetButtonDown("Jump");
            Input.JumpUp = UnityEngine.Input.GetButtonUp("Jump");
            Input.X = UnityEngine.Input.GetAxisRaw("Horizontal");
            if (Input.JumpDown)
            {
                _JumpBuffer=true;
                _TimePressedJump = Time.time;
            }
            if (_CurrentHorizontalSpeed != 0) transform.localScale = new Vector3(Mathf.Sign(_CurrentHorizontalSpeed), 1, 1);
            //Debug.Log($"Update {UpdateCount++}");
            //UpdateAnimation();
        }
        private static InputCollection Input;
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
 
            //Debug.Log($"FixedUpdate {FixedUpdateCount++}");
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
        [Tooltip("Decrease when jumping up")]
        [SerializeField] float JumpDecrease;
        [SerializeField] float JumpSpeed;
        [Tooltip("Early Fall Speed")]
        [SerializeField] float FallSpeed;
        [Tooltip("Decrease when falling down")]
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
            else if (!_CollisionLeft && Input.X <0)
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
        List<RaycastHit2D> _RaycastHits =new List<RaycastHit2D>();
        #region Move 
        void Move() {
            
            var move = new Vector2(_CurrentHorizontalSpeed, _CurrentVerticalSpeed);
            Debug.Log($"initial Move {move}");
            /*rigbody.Cast(move, _RaycastHits, move.magnitude*Time.fixedDeltaTime);
            if (_RaycastHits.Count != 0) {
                move=Vector2.zero;
            }*/
           rigbody.MovePosition(rigbody.position + move * Time.fixedDeltaTime);

        }
        #endregion
        #region Gizmos
        private void OnDrawGizmos()
        {
            // Bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + _CharacterBounds.center, _CharacterBounds.size);


            if (!Application.isPlaying) return;

            // Draw the future position. Handy for visualizing gravity
            Gizmos.color = Color.red;
            var move = new Vector3(_CurrentHorizontalSpeed, _CurrentVerticalSpeed) * Time.deltaTime;
            Gizmos.DrawWireCube(transform.position + _CharacterBounds.center + move, _CharacterBounds.size);
        }
            
        #endregion
    }


}
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
            animator =GetComponent<Animator>();
        }
        /// <summary>
        /// Receive input 
        /// </summary>
        
        void Update()
        {
            var Input = new InputCollection{
                JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = UnityEngine.Input.GetButtonUp("Jump"),
                X = UnityEngine.Input.GetAxisRaw("Horizontal")
            };
            if (Input.JumpDown)
            {
               _TimePressedJump = Time.time;
            }
            
        }
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
            //collision 
            //status update 
            //move
            //gravity
            //jump
            //actually move 
            //animation 
        }

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
            

            
            bool RunDetection(Vector2 start,Vector2 end,Vector2 Direction)
            {
                return EvaluateRayPositions(start,end).Any(point => Physics2D.Raycast(point, Direction, _DetectionRayLength, _GroundLayer));
            }
        }
        private IEnumerable<Vector2> EvaluateRayPositions(Vector2 start,Vector2 end)
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
        private bool CanUseCoyote => _CoyoteUsable && ! _CollisionDown&& Time.time - _TimeLeftGround < CoyoteAllowTime;
        private float _TimeLeftGround;
        bool EndJumpEarly;
        [SerializeField] float JumpHeight;
        [SerializeField] float JumpSpeed;
        [SerializeField] float MaxFallSpeed;

        void CalculateJump() {
            if (_CollisionDown)
            {
                // Move out of the ground
                
            }
            /*if (CanUseCoyote || _JumpBuffer)
            {
                _currentVerticalSpeed = _jumpHeight;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                _timeLeftGrounded = float.MinValue;
                JumpingThisFrame = true;
            }
            else
            {
                JumpingThisFrame = false;
            }

            // End the jump early if button released
            if (!_colDown && Input.JumpUp && !_endedJumpEarly && Velocity.y > 0)
            {
                // _currentVerticalSpeed = 0;
                _endedJumpEarly = true;
            }

            if (_colUp)
            {
                if (_currentVerticalSpeed > 0) _currentVerticalSpeed = 0;
            }*/
        }
        #endregion
    }

}
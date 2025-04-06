using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour, IPlayerInput
    {
        private CapsuleCollider _col;
        private Rigidbody _rb;
        private bool _grounded;
        private bool _isCoyote;
        private bool _jumpOnCooldown;
        private Vector3 _groundNormal = Vector3.up;
        
        // Input
        private Vector2 _moveVector;
        private bool _jumping = false;
        private bool _crouching = false;

        [Header("Movement Variables")] public float moveSpeed = 5f;
        public float acceleration = 2;
        public float airSpeed = 1;
        public float airAcceleration = 1;
        public float crouchSpeedMultiplier = 0.5f;
        public float jumpForce = 5f;
        public float unGroundDelay = 0.1f;
        [Header("Crouching")] public float crouchSharpness = 20f;
        public float playerStandHeight = 1.65f;
        public float playerCrouchHeight = 1f;
        public float crouchAirRatio = 0.5f;
        public bool smoothCrouch = true;
        [Header("Other movement variables")] public float gravityScale = 9.82f;
        public bool canMove = true;

        [Header("Move Limitations")] public float jumpCooldown = 0.3f;
        public float friction = 0.6f;
        public float maxStrafeSpeed = 30;
        public float maxGroundAngle = 35f;
        public float maxVelocity = 50;
        public LayerMask walkableLayers;
        
        [Header("References")]
        public Transform body;


        private float _regularSpeed;

        private void Awake()
        {
            _col = GetComponent<CapsuleCollider>();
            _rb = GetComponent<Rigidbody>();

            _targetPlayerHeight = playerStandHeight;
            UpdatePlayerHeight(_targetPlayerHeight);
            _regularSpeed = moveSpeed;
        }

        void FixedUpdate()
        {
            CheckCrouch();
            UpdatePlayerHeight(_targetPlayerHeight, smoothCrouch);
            Movement();
        }
        
        private void OnEnable()
        {
            this.Subscribe();


        }

        private void OnDisable()
        {
            this.UnSubscribe();
        }
        public bool IsMoving()
        {
            return _moveVector.sqrMagnitude > 0.01f;
        }
        public void OnMove(Vector2 value)
        {
            UnityEngine.Debug.Log(value);
            _moveVector = value;
        }

        
        public void OnJump(InputAction.CallbackContext context)
        {
            _jumping = context.performed;
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            _crouching = context.performed;
        }

        private Vector3 _endVel;

        private void Movement()
        {
            _endVel = body.InverseTransformVector(_rb.linearVelocity);

            if (canMove)
            {
                if (_jumping)
                {
                    if (CanJump())
                    {
                        //StartCoroutine(JumpCooldown());
                        Jump();
                    }
                }

                if (_grounded)
                {
                    _endVel = Accelerate(_endVel, moveSpeed, acceleration, _groundNormal);
                    _endVel = Friction(_endVel, moveSpeed, friction, _groundNormal);
                }
                else //Note to self: gör så att air movement utgår mer från ens velocity som man redan har. eller uhh, svårt att förklara
                {
                    _endVel = Accelerate(_endVel, airSpeed, airAcceleration, Vector3.up);
                }
            }

            if (!_grounded)
            {
                Gravity();
            }

            _rb.linearVelocity = body.TransformVector(_endVel);
        }

        private Vector3 Accelerate(Vector3 vel, float wishSpeed, float accel, Vector3 normal)
        {
            Vector3 v3Move = new Vector3(_moveVector.x, 0, _moveVector.y);
            Vector3 moveRight = Vector3.Cross(v3Move, Vector3.up); // this might have issues ---------------------------
            Vector3 planeMove = Vector3.Cross(normal, moveRight).normalized;

            float currentSpeed = Vector3.Dot(vel, planeMove); //tf does this mean
            float addSpeed = Mathf.Max(wishSpeed - currentSpeed, 0);
            float accelSpeed = Mathf.Min(accel * wishSpeed, addSpeed);
            Vector3 clampedSpeed =
                Vector3.ClampMagnitude(new Vector3(vel.x, 0, vel.z) + planeMove * accelSpeed, maxStrafeSpeed);
            return clampedSpeed + Vector3.up * vel.y;
        }

        private Vector3 Friction(Vector3 vel, float wishSpeed, float friction, Vector3 normal)
        {
            Vector3 v3Move = new Vector3(_moveVector.x, 0, _moveVector.y);
            Vector3 moveRight = Vector3.Cross(v3Move, Vector3.up);  // this might also have issues -------------------------------
            Vector3 planeMove = Vector3.Cross(normal, moveRight).normalized;
            Vector3 overDesired = vel - planeMove * wishSpeed;

            float frictionCheck = Vector3.Dot(vel, overDesired);
            if (frictionCheck <= 0)
                return vel;

            Vector3 frictionAdd = -overDesired * Mathf.Clamp01(friction);
            return vel + frictionAdd;
        }

        private void Jump()
        {
            _isCoyote = false;
            _endVel.y = jumpForce;
        }

        private bool CanJump()
        {
            return (_isCoyote || _grounded) && !_jumpOnCooldown; //fix jumpcooldown
        }

        IEnumerator JumpCooldown()
        {
            _jumpOnCooldown = true;
            yield return new WaitForSeconds(jumpCooldown);
            _jumpOnCooldown = false;
        }

        private bool _isCrouching;
        private float _targetPlayerHeight;

        private void ToggleCrouch(float newHeight)
        {
            _targetPlayerHeight = newHeight;
            //Fråga Leo om det här är ett bra sätt att göra det
            if (_isCrouching)
            {
                moveSpeed *= crouchSpeedMultiplier;
            }
            else
            {
                moveSpeed = _regularSpeed;
            }
        }

        private void CheckCrouch()
        {
            if (_crouching)
            {
                if (!_isCrouching)
                {
                    _isCrouching = true;
                    ToggleCrouch(playerCrouchHeight);
                }
            }
            else if (_isCrouching) //Add uncrouch check
            {
                _isCrouching = false;
                ToggleCrouch(playerStandHeight);
            }
        }

        private void UpdatePlayerHeight(float height, bool smooth = false)
        {
            if (smooth)
            {
                float heightFrom = _col.height;
                _col.height = Mathf.Lerp(_col.height, height, crouchSharpness * Time.fixedDeltaTime);
                _col.center = (_col.height / 2) * Vector3.up;
                if (!_grounded)
                {
                    transform.position += (heightFrom - _col.height) * crouchAirRatio * Vector3.up;
                }
            }
            else
            {
                _col.height = height;
                _col.center = (_col.height / 2) * Vector3.up;
            }
        }

        private void Gravity()
        {
            _endVel += Vector3.down * (gravityScale * Time.fixedDeltaTime);
        }

        private Coroutine _currentCoyote;

        private IEnumerator CoyoteBuffer(float buffer)
        {
            _isCoyote = true;
            yield return new WaitForSeconds(buffer);
            _isCoyote = false;
        }

        private void OnCollisionExit(Collision col)
        {
            if (!_grounded) return;
            _grounded = false;
            _currentCoyote = StartCoroutine(CoyoteBuffer(unGroundDelay));
        }

        private void OnCollisionStay(Collision col)
        {
            for (int i = 0; i < col.contactCount; i++)
            {
                if (IsWithinAngle(col.GetContact(i).normal, maxGroundAngle))
                {
                    _grounded = true;
                    if (_currentCoyote != null)
                        StopCoroutine(_currentCoyote);
                    _groundNormal = col.GetContact(i).normal;
                }
            }
        }

        private bool IsWithinAngle(Vector3 normal, float angle)
        {
            return Vector3.Angle(normal, Vector3.up) <= angle;
        }

        public float GetHeight()
        {
            return _col.height;
        }
    }
}
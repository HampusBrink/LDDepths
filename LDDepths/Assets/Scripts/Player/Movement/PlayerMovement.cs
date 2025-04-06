using System.Diagnostics;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour, IPlayerInput
    {
        [SerializeField] private float moveSpeed = 5f;
        private Rigidbody _rb;
        private Vector2 _moveVector;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
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

        private void FixedUpdate()
        {
            var moveVector = (transform.right * _moveVector.x + transform.forward * _moveVector.y).normalized;
            _rb.linearVelocity = new Vector3(moveVector.x * moveSpeed,_rb.linearVelocity.y,moveVector.z * moveSpeed);
        }
    }
}
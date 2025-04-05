using Input;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour, IPlayerInput
    {
        [SerializeField] private float moveSpeed = 5f;
        private Rigidbody2D _rb;
        private Vector2 _moveVector;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            this.Subscribe();
        }

        private void OnDisable()
        {
            this.UnSubscribe();
        }

        public void OnMove(Vector2 value)
        {
            _moveVector = value;
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _moveVector.normalized * moveSpeed;
        }
    }
}
using UnityEngine;

namespace Game.Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PlayerController2D : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4f;
        private Rigidbody2D _rb = null!;
        private Vector2 _input;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (_input.sqrMagnitude > 1f) _input.Normalize();
        }

        private void FixedUpdate()
        {
            _rb.velocity = _input * moveSpeed;
        }
    }
}

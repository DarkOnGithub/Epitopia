using Unity.Netcode;
using UnityEngine;

namespace Players
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;

        private float horizontal;
        private bool isFacingRight = true;
        private readonly float jumpingPower = 16f;
        private readonly float speed = 8f;


        private void FixedUpdate()
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
            horizontal = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space)) rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);

            if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0f)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            if (Camera.main != null && transform != null)
            {
                Camera.main.transform.position =
                    new Vector3(transform.position.x, transform.position.y, -10f);
                if (PlayerManager.LocalPlayer != null)
                    PlayerManager.LocalPlayer.Position = transform.position;
            }

            Flip();
        }

        private bool IsGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, 2f, groundLayer);
        }

        private void Flip()
        {
            if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
            {
                isFacingRight = !isFacingRight;
                var localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
    }
}
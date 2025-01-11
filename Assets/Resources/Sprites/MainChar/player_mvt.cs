// namespace DefaultNamespace;
// using UnityEngine;
//
// public class player_mvt
// {
//     public int speed;
//     private Rigidbody2D rb;
//     public int JumpPower;
//     
//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//     }
//
//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Q))
//         {
//             rb.linearVelocity =new Vector2(-speed, rb.linearVelocity.y);
//         }
//         if (Input.GetKeyDown(KeyCode.D))
//         {
//             rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
//         }
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
//         }
//     }
// }
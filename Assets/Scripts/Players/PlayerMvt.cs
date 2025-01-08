using UnityEngine;

public class PlayerMvt : MonoBehaviour
{
    public int speed;
    private Rigidbody2D rb;
    public int JumpPower;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rb.linearVelocity =new Vector2(-speed * 2, rb.linearVelocity.y);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.linearVelocity = new Vector2(speed * 2, rb.linearVelocity.y);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
        }
    }
}

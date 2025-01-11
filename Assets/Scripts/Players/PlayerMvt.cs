using UnityEngine;

public class PlayerMvt : MonoBehaviour
{
    public int speed;
    private Rigidbody2D rb;
    public int JumpPower;

   // public Sprite spriteStart;

    //[SerializeField] private Sprite[] mvtFace;
    //public Sprite actualFace;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) //vers left
        {
            rb.linearVelocity =new Vector2(-speed * 2, rb.linearVelocity.y);
            //actualFace = mvtFace[0];
        }
        if (Input.GetKeyDown(KeyCode.D)) //vers right
        {
            rb.linearVelocity = new Vector2(speed * 2, rb.linearVelocity.y);
            //actualFace = mvtFace[1];
        }
        if (Input.GetKeyDown(KeyCode.Space))  //jump
        {
            rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
           // actualFace = mvtFace[2];
        }
       // GetComponent<SpriteRenderer>().sprite = actualFace;
    }
}

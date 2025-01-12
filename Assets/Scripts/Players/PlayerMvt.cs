using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class PlayerMvt : MonoBehaviour
{
    public int speed;
    private Rigidbody2D rb;
    public int JumpPower;
    

    //1 = vers droite, -1 = vers gauche (pour animation perso)
    
    
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
           // PersoDirection = -1;

        }
        if (Input.GetKeyDown(KeyCode.D)) //vers right
        {
            rb.linearVelocity = new Vector2(speed * 2, rb.linearVelocity.y);
            //PersoDirection = 1
        }
        if (Input.GetKeyDown(KeyCode.Space))  //jump
        {
            rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
        }
    }
}

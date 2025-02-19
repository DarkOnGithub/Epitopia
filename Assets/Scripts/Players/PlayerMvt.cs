using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMvt : MonoBehaviour
{
    public int speed;
    private Rigidbody2D rb;
    public int JumpPower;
    

    [field: SerializeField] public Mvtanimation1 Direction { get; private set; }
    private Animator _animator;
    private Mvtanimation1 _currentDirection;
    private Vector2 _axisInput = Vector2.zero;
    
    private Vector2 direction;

    private AnimationClip _currentClip;
    //1 = vers droite, -1 = vers gauche (pour animation perso)
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentDirection = Direction;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector2.right;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            direction = Vector2.left;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            direction = Vector2.up; 
        }
        
        rb.linearVelocity = direction * speed;
        
        
        
        
        
        
        
        
        
        
        
        /*

        AnimationClip expectedClip = _currentDirection.GetFacingClip(_axisInput);
        if (_currentClip is null || _currentClip != expectedClip)
        {
            _animator.Play(expectedClip.name);
        }
        
        
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
        */
    }
}

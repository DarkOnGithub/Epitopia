using System;
using System.Runtime.Remoting.Contexts;
using QFSW.QC.Actions;
using UnityEngine;


public class finalmvt : MonoBehaviour
{

    [field: SerializeField] public float Speed { get; set; } = 5f;
    [field: SerializeField] public float fallMultiplier { get; set; }
    private Rigidbody2D _rigidbody2D;
    public int jumpPower;
    private Vector2 move;
    private Vector2 vecGravity;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }

        if (Input.GetButtonDown("Jump"))
        {
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, jumpPower);
        }

        
        if (_rigidbody2D.linearVelocity.y < 0 )
        {
            _rigidbody2D.velocity -= vecGravity;
            //plus tard pour une compétence d'attaque au sol
        }
    }

    
    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = new Vector2(move.x * Speed, _rigidbody2D.linearVelocity.y);
    }
    
    
}

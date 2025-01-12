using System.Threading.Tasks;
using Players;
using Unity.Netcode;
using UnityEngine;
using World;

public class PlayerController : NetworkBehaviour
{
    public int speed;
    private Rigidbody2D rb;
    public int JumpPower;
    private float _horizontalFactor;
    private Camera _camera;

    public override async void OnNetworkSpawn()
    {
        if (!IsLocalPlayer) {
            GetComponent<Camera>().enabled = false;
            return; // Ensure only the local player initializes the camera and scanner
        }
        
        _camera = GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        await Task.Delay(2000);
        Scanner.Instance.InitializeScanner(_camera);
    }

    void Update()
    {
        if (!IsLocalPlayer) return; // Ensure only the local player processes input and updates the camera
        if (PlayerManager.LocalPlayer == null) return;
        if (_camera == null) return;
        _horizontalFactor = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(_horizontalFactor * speed, rb.linearVelocity.y);
            
        if (Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);

        PlayerManager.LocalPlayer.Position = transform.position;
        _camera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    private bool IsGrounded()
    {
        // Implement ground check logic here
        return true;
    }
}
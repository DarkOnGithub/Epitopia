using System.Threading.Tasks;
using Network.Messages;
using Network.Messages.Packets.Players;
using Players;
using Unity.Netcode;
using UnityEngine;
using World;

namespace Players
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] public int speed;
        [SerializeField] public int jumpPower;

        private Rigidbody2D _rigidbody2D;
        private float _horizontalFactor;
        private Player _localPlayer;
        private Vector2 _facingLeft;

        private bool _isFacingLeft;

        public override void OnNetworkSpawn()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _localPlayer = PlayerManager.LocalPlayer;
            _facingLeft = new Vector2(2.5f, 2.5f);
            transform.localScale = _facingLeft;
            _isFacingLeft = true;
        }


        private void Update()
        {
            if (_localPlayer == null)
            {
                _localPlayer = PlayerManager.LocalPlayer;
                return;
            }

            _horizontalFactor = Input.GetAxisRaw("Horizontal");
            _rigidbody2D.linearVelocity = new Vector2(_horizontalFactor * speed, _rigidbody2D.linearVelocity.y);
            if (_horizontalFactor > 0 && _isFacingLeft)
            {
                _isFacingLeft = false;
                Flip();
            }

            if (_horizontalFactor < 0 && !_isFacingLeft)
            {
                _isFacingLeft = true;
                Flip();
            }

            if (Input.GetKeyDown(KeyCode.Space)) _rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            _localPlayer.Position = transform.position;
        }

        protected virtual void Flip()
        {
            if (_isFacingLeft)
                MessageFactory.SendPacket(SendingMode.ClientToClient, new PlayerControllerMessage
                                                                      {
                                                                          Scale = _facingLeft,
                                                                          NetworkObjectId =
                                                                              GetComponent<NetworkObject>()
                                                                                 .NetworkObjectId
                                                                      });
            //transform.localScale = _facingLeft;
            if (!_isFacingLeft)
                MessageFactory.SendPacket(SendingMode.ClientToClient, new PlayerControllerMessage
                                                                      {
                                                                          Scale = new Vector2(
                                                                              -transform.localScale.x,
                                                                              transform.localScale.y),
                                                                          NetworkObjectId =
                                                                              GetComponent<NetworkObject>()
                                                                                 .NetworkObjectId
                                                                      });
            //transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }
}
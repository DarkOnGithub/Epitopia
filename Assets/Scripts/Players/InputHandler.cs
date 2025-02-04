using Core;
using Events.Events;
using Network.Messages;
using Network.Messages.Packets.World;
using UnityEngine;
using World;
using World.Blocks;

namespace Players
{
    public class InputHandler
    {
        private ClientWorldHandler _clientWorldHandler;
        private UnityEngine.Camera _camera;
        public InputHandler(ClientWorldHandler clientWorldHandler)
        {
            _clientWorldHandler = clientWorldHandler;
            InputManager.BindNewMouse(KeyCode.Mouse0, OnMouseClick);
            _camera = UnityEngine.Camera.main;
        }
        
        public void OnMouseClick(OnMouseEvent @event)
        {
            var worldPosition = _camera.ScreenToWorldPoint(@event.Position);
            MessageFactory.SendPacket(SendingMode.ClientToServer, new BlockActionMessage()
                                                                    {
                                                                        Position = new Vector2Int((int)worldPosition.x, (int)worldPosition.y),
                                                                        Type = BlockActionType.Place,
                                                                        World = _clientWorldHandler.WorldIn.Identifier,
                                                                        BlockState = BlockRegistry.BLOCK_STONE.GetDefaultState()
                                                                    });   
        }
    }
}
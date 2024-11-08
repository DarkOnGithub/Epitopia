using Unity.Services.Authentication;
using UnityEngine;
using World;
using World.Worlds;

namespace Players
{
    public class Player
    {
        public World.World World { get; set; }
        public string PlayerId { get; set; }
        public string Username { get; set; }
        public ulong ClientId { get; set; }
        public Vector2 Position { get; set; }
        public Player()
        {
            World = Overworld.Instance;
            WorldManager.PlayersWorld.Add(ClientId, World);
        }
        
        
    }
}
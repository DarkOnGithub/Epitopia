using Unity.Netcode;
using UnityEngine;
using World;

namespace Players
{
    public class Player
    {
        public ulong ClientId;
        public string PlayerId;
        public string PlayerName;
        public Vector2 Position;
        public AbstractWorld World;
        public GameObject PlayerGameObject;
        private NetworkObject _networkObject;
        
        public Player(string playerName, string playerId, ulong clientId)
        {
            PlayerName = playerName;
            PlayerId = playerId;
            ClientId = clientId;
            World = WorldManager.GetWorld(WorldIdentifier.Overworld);
            PlayerManager.Players.Add(this);
           
        }

        public void Spawn(Vector2Int position = default)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                var y = World.WorldGenerator.GetHeightAt(0) + 4;
                PlayerGameObject = GameObject.Instantiate(Resources.Load<GameObject>("Sprites/MainChar/PrefabChar/PlayerSwitchSide"));
                _networkObject = PlayerGameObject.GetComponent<NetworkObject>();
                PlayerGameObject.transform.position = new(0, y, 0);
                PlayerGameObject.transform.localScale = new(2.5f, 2.5f, 1);
                _networkObject.SpawnWithOwnership(ClientId);
            }
        }
    }
}
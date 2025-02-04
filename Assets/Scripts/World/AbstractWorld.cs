// using System;
// using Core;
// using Unity.Netcode;
// using World.WorldGeneration;
//
// namespace World
// {
//     public abstract class AbstractWorld : IDisposable
//     {
//         private readonly ServerWorldHandler _serverHandler;
//         public readonly WorldIdentifier Identifier;
//         private bool _disposed;
//         protected BetterLogger Logger = new(typeof(AbstractWorld));
//
//         public WorldQuery Query;
//         public WorldGenerator WorldGenerator;
//
//         
//         public ClientWorldHandler ClientHandler { get; private set; }
//
//         public ServerWorldHandler ServerHandler
//         {
//             get
//             {
//                 if (!NetworkManager.Singleton.IsHost)
//                     Logger.LogWarning("ServerHandler is only available on the server");
//                 return _serverHandler;
//             }
//         }
//         
//         public AbstractWorld(WorldIdentifier identifier)
//         {
//             Identifier = identifier;
//             Query = new WorldQuery(this);
//             ClientHandler = new ClientWorldHandler(this);
//             if (NetworkManager.Singleton.IsHost)
//             {
//                 _serverHandler = new ServerWorldHandler(this);
//                 WorldGenerator = new WorldGenerator(this);
//             }
//         }
//
//
//         public void Dispose()
//         {
//             Dispose(true);
//             GC.SuppressFinalize(this);
//         }
//         protected virtual void Dispose(bool disposing)
//         {
//             if (_disposed)
//                 return;
//
//             if (disposing) ClientHandler = null;
//
//             _disposed = true;
//         }
//         ~AbstractWorld()
//         {
//             Dispose(false);
//         }
//     }
// }

using Unity.Netcode;

namespace World
{
    public abstract class AbstractWorld
    {
        public readonly WorldIdentifier Identifier;
        public readonly ClientWorldHandler ClientHandler;
        public readonly ServerWorldHandler ServerHandler;
        
        
        private bool _isHost;
            
        protected AbstractWorld(WorldIdentifier identifier)
        {
            _isHost = NetworkManager.Singleton.IsHost;
            
            Identifier = identifier;
            
            ClientHandler = new ClientWorldHandler(this);
            if (_isHost)
                ServerHandler = new ServerWorldHandler(this);
        }
    }
}
    

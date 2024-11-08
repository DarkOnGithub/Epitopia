using UnityEngine.Tilemaps;

namespace World.Worlds
{
    public class Overworld : World
    {
        public static Overworld Instance { get; private set; }
        public Overworld() : base()
        {
            Instance = this;
        }
    }
}
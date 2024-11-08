using System.Collections.Generic;

namespace Players
{
    public class PlayerManager
    {
        public static Player LocalPlayer { get; set; }
        public static List<Player> Players = new();
    }
}
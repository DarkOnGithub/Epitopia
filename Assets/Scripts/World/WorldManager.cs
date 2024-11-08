using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World
{
    public class WorldManager : MonoBehaviour
    {
        public static Dictionary<ulong, World> PlayersWorld = new();
        public static WorldManager Instance { get; private set; }
        [SerializeField]
        public Tilemap tilemap;
        [SerializeField]
        public Grid grid;
        private void Awake()
        {
            Instance = this;
        }
    }
}
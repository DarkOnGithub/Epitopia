using System;
using System.Linq;
using Entities;
using Entities.Entity;
using Players;
using QFSW.QC;
using Tiles;
using UnityEditor;
using UnityEngine;
using Utils;
using World.Blocks;
using World.Chunks;

namespace Core.Commands
{
    public static class TestCommands
    {
        [Command]
        public static void AddEntity()
        {
            var entity = new DummyEntity(PlayerManager.LocalPlayer.World);
            entity.Spawn(PlayerManager.LocalPlayer.Position);
        }
    }
}
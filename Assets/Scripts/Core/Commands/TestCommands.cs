using System;
using System.Linq;
using Entities;
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
        // [Command]
        // public static void AddEntity()
        // {
        //     var entity = new BaseEntity(PlayerManager.LocalPlayer.World);
        //     entity.Spawn(PlayerManager.LocalPlayer.Position);
        //     entity.EntityBehaviour.SetPath(entity.PathFinder.FindPath(entity.Prefab.transform.position, Vector3.ve + new Vector2Int(100, 10)));
        // }
    }
}
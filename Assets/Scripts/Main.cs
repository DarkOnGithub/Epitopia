using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Blocks;
using Core.Commands;
using Events;
using Events.EventHandler;
using MessagePack;
using MessagePack.Resolvers;
using Network.Messages;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils.LZ4;
using World.Chunks;
using World.Worlds;
using Event = UnityEngine.Event;


public class Main : MonoBehaviour
{
    public IEnumerator Temp()
    {
        yield return new WaitForSeconds(5);
        WorldCommands.SendChunk();
    }
    private async void Start()
    {
        var world = new Overworld();
        var chunk = world.GenerateChunk(new Vector2Int(0, 0));
        for (int i = -16; i < 16; i++)
        {
            for(int z = -16; z < 16; z++)
            {
                world.SetBlock(new Vector2Int(i, z), BlocksRegistry.BLOCK_DIRT.CreateBlockData());
            }
        }    
        chunk.Draw();
        StartCoroutine(Temp());
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Core.Commands;
using Events;
using Events.EventHandler;
using MessagePack;
using MessagePack.Resolvers;
using Network.Messages;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils.LZ4;
using World.Blocks;
using World.Chunks;
using Event = UnityEngine.Event;


public class Main : MonoBehaviour
{

    private async void Start()
    {
        BlockRegistry.RegisterBlocks();
        //StartCoroutine(Temp());
    }
}
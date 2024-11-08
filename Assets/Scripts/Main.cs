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
using World.Chunks;
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

        //StartCoroutine(Temp());
    }
}
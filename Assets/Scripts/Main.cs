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
using Players;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils.LZ4;
using World.Blocks;
using World.Chunks;
using Event = UnityEngine.Event;


public class Main : MonoBehaviour
{
    IEnumerator Temp()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / 20f);
            if(PlayerManager.LocalPlayer == null) continue;
            Camera.main.transform.position += new Vector3(2, 0, 0);
            PlayerManager.LocalPlayer.Position = Camera.main.transform.position;
        }
    }
    private async void Start()
    {
        BlockRegistry.RegisterBlocks();
        StartCoroutine(Temp());
    }
}
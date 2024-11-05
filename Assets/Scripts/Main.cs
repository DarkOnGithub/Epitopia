using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Core.Commands;
using Events;
using Events.EventHandler;
using Network.Packets;
using UnityEngine;
using Event = UnityEngine.Event;


public class Main : MonoBehaviour
{
    private IEnumerator Temp()
    {
        yield return new WaitForSeconds(3);
       
    
        Packets.SendPacket("hello");
    }
    private async void Start()
    {
        new TestPacket();
        await LobbyCommands.CreateLobby("hello");
        StartCoroutine(Temp());
    }
}
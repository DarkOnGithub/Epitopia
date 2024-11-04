using System;
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
    private async void Start()
    {
        new TestPacket();
        await LobbyCommands.CreateLobby("hello");
        PacketFactory.Initialize();

        Packets.SendPacket("hello");
    }
}
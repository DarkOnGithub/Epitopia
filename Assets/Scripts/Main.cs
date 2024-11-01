using System;
using System.Collections.Generic;
using System.Reflection;
using Events;
using Events.EventHandler;
using Gui.Menus;
using Tests;
using UnityEngine;
using Event = UnityEngine.Event;


public class Main : MonoBehaviour
{
    private  void Awake()
    {
        new LobbyMenu().Show();
    }
}

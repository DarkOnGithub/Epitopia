using System;
using UIs;
using UnityEngine;



public class Main : MonoBehaviour
{
    private void Awake()
    {
        var lobbyMenu = UIManager.Instanciate<LobbyMenu>();
        UIManager.LoadWindow(lobbyMenu);
    }
}

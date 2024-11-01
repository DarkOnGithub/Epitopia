using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Gui.Menus
{
    public class LobbyMenu : Gui
    {
        public LobbyMenu() : base("LobbyMenu")
        {
            Debug.Log("a");
            RootVisualElement.Q<Button>("CreateServer").RegisterCallback<ClickEvent>(async (evt) => 
            {
                var code = await LobbyManager.StartHostAndGetJoinCode();
                Debug.Log(code);
            });
            
            RootVisualElement.Q<Button>("JoinServer").RegisterCallback<ClickEvent>(async (evt) => 
            {
                await LobbyManager.StartClient(RootVisualElement.Q<TextField>("JoinCode").value);
            });
            
            
            
        }
    }
}
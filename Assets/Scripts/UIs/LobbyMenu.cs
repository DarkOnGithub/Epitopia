using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace UIs
{
    public class LobbyMenu : UserInterface
    {
        public LobbyMenu()
        {
        }

        protected override void OnLoad()
        {
            var joinCodeField = RootVisualElement.Q<TextField>("JoinCode");
            var joinServerButton = RootVisualElement.Q<Button>("JoinServer");
            var createServerButton = RootVisualElement.Q<Button>("CreateServer");

            joinServerButton.RegisterCallback<ClickEvent>(async _ =>
            {
                await LobbyManager.StartClient(joinCodeField.value);
            });
            createServerButton.RegisterCallback<ClickEvent>(async _ =>
            {
                var code = await LobbyManager.StartHostAndGetJoinCode();
                Debug.Log(code);
            });
        }
        
    }
}
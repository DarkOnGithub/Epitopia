using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Commands;
using Network.Client;
using Network.Server;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PanelType
{
    None,
    Main,
    Option,
    Credits,
}
public class MenuController : MonoBehaviour
{
    [Header("Panels")] [SerializeField] private TMP_InputField field;
    [SerializeField] private List<MenuPanel> panelsList = new List<MenuPanel>();
    private Dictionary<PanelType, MenuPanel> panelsDict = new Dictionary<PanelType, MenuPanel>();

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;

        foreach (MenuPanel _panel in panelsList) 
        {
            if (_panel)
            {
                panelsDict.Add(_panel.GetPanelType(), _panel);
            }
        }


        OpenOnePanel(PanelType.Main);
    }

    private void OpenOnePanel(PanelType _type)
    {
        foreach (MenuPanel _panel in panelsList)
        {
            _panel.ChangeState(false);
        }

        if (_type != PanelType.None)
        {
            panelsDict[_type].ChangeState(true);
        }
    }
    public void OpenPanel(PanelType _type)
    {
        OpenOnePanel(_type);
    }
    public async void ChangeScene(string _sceneName)
   {
       SceneManager.LoadScene("Scenes/Workspace");
       await LobbyManager.CreateLobby("TestServer", 4);
       Server.CreateInstance(name);
   }
    public async void CreateServer()
    {
        if (field.text != "")
        {
            SceneManager.LoadScene("Scenes/Workspace");
            await Task.Delay(2000);
            await LobbyManager.CreateLobby(field.text, 4);
            Server.CreateInstance(field.text);
        }
    }
    public async void JoinServer()
    {
        if (field.text != "")
        {
            SceneManager.LoadScene("Scenes/Workspace");
            await Task.Delay(2000);

            await LobbyCommands.JoinLobbyFromName(field.text);
            Client.CreateInstance();
        }
    }
    public void Quit()
    {
        gameManager.Quit();
    }
}

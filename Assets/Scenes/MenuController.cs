using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PanelType
{
    None,
    Main,
    Option,
    Credits,
}
public class MenuController : MonoBehaviour
{
    [Header("Panels")]
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
    public void ChangeScene(string _sceneName)
   {
        SceneManager.LoadScene(_sceneName);
   }

    public void Quit()
    {
        gameManager.Quit();
    }
}

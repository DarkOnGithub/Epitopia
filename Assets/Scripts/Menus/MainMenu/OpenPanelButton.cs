using UnityEngine;

public class OpenPanelButton : MonoBehaviour
{
    [SerializeField] private PanelType type;

    private MenuController controller;

    private void Start()
    {
        controller = FindObjectOfType<MenuController>();
    }

    public void OnClick()
    {
        controller.OpenPanel(type);
    }
}
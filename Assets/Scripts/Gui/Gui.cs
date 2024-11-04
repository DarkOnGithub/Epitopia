using UnityEngine;
using UnityEngine.UIElements;

namespace Gui
{
    public class Gui
    {
        public GameObject GameObject;
        protected VisualTreeAsset Asset;
        protected UIDocument Document;
        public VisualElement RootVisualElement;
        public PanelSettings Settings = Resources.Load<PanelSettings>("UIs/UI Toolkit/PanelSettings");
        /// <summary>
        /// A GUI object
        /// </summary>
        /// <param name="guiName">Path to its ui toolkit files</param>
        public Gui(string guiName)
        {
            GameObject = new GameObject(guiName);
            Asset = Resources.Load<VisualTreeAsset>($"UIs/{guiName}/{guiName}");
            Document = GameObject.AddComponent<UIDocument>();
            Document.panelSettings = Settings;
            Document.visualTreeAsset = Asset;
            RootVisualElement = Document.rootVisualElement;
        }

        public void Show()
        {
            RootVisualElement.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            RootVisualElement.style.display = DisplayStyle.None;
        }
    }
}
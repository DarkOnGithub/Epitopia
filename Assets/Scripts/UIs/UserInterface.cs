using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIs
{
    public abstract class UserInterface
    {
        public GameObject GameObject;
        private readonly VisualTreeAsset _asset; 
        private UIDocument _document;
        protected  VisualElement RootVisualElement;
        private static readonly PanelSettings Settings = Resources.Load<PanelSettings>("UIs/UI Toolkit/PanelSettings");
        
        protected abstract void OnLoad();
        
        public UserInterface()
        {
            var name = GetType().Name;
            _asset = Resources.Load<VisualTreeAsset>($"UIs/{name}/{name}");
            if (_asset == null)
                throw new FileNotFoundException($"Interface: {name} not found");
        }
        public void Load()
        {
            _document = GameObject.AddComponent<UIDocument>();
            _document.visualTreeAsset = _asset;
            _document.panelSettings = Settings;
            RootVisualElement = _document.rootVisualElement;
            Debug.Log($"Opening GUI: {GetType().Name}");
            OnLoad();
        }
      
    }
}
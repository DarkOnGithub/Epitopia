using System;
using Events.Events;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public static class InputManager
    {
        
        private static InputAction _mouseAction = new InputAction("Mouse", InputActionType.Value);
        private static InputAction _keyboardAction = new InputAction("Keyboard", InputActionType.Value, "<Keyboard>/all");
        public static void BindNewKeyboard(Key key, Action<OnKeyEvent> action)
        {
            OnKeyEvent.Registry += (action, key);
            var inputAction = new InputAction("Press " + key, InputActionType.Button, $"<Keyboard>/{key.ToString().ToLower()}");
            inputAction.performed += ctx =>
            {
                action(new OnKeyEvent
                       {
                           Ctx =  ctx,
                           Key = key
                       });
            };
            inputAction.Enable();
        }
        
        public static void BindNewMouse(KeyCode code, Action<OnMouseEvent> action)
        {
            
            OnMouseEvent.Registry += (action, code);
            var inputAction = new InputAction("Press" + code, InputActionType.Button, $"<Mouse>/{GetMouseButtonPath(code)}");
            inputAction.performed += ctx =>
            {
                action(new OnMouseEvent
                       {
                           Ctx =  ctx,
                           Mouse = code,
                           Position = Mouse.current.position.ReadValue()
                       });
            };
            inputAction.Enable();
        }
        private static string GetMouseButtonPath(KeyCode code)
        {
            switch (code)
            {
                case KeyCode.Mouse0:
                    return "leftButton";
                case KeyCode.Mouse1:
                    return "rightButton";
                case KeyCode.Mouse2:
                    return "middleButton";
                case KeyCode.Mouse5:
                    return "delta";
                default:
                    return null;
            }
        }
    }
}
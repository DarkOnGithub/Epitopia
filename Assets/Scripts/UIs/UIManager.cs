using System;
using UIs;
using UnityEngine;

public class UIManager
{
    public static T Instanciate<T>() where T: UserInterface
    {
        var gameObject = new GameObject(typeof(T).Name);
        var instance = Activator.CreateInstance<T>();
        instance.GameObject = gameObject;
        return instance;
    }
    
    public static T LoadWindow<T>() where T : UserInterface
    {
        var window = Instanciate<T>();
        window.Load();
        return window;
    }

    public static void LoadWindow(UserInterface userInterface)
    {
        userInterface.Load();
    }
}

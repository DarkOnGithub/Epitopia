using System.Collections;
using Core;
using Events.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using World.Blocks;
using World.WorldGeneration;

public class Main : MonoBehaviour
{
    public static Main instance;

    [SerializeField] public RuleTile ruleTile;

    private async void Start()
    {
        instance = this;
        BlockRegistry.RegisterBlocks();

        InputManager.BindNewKeyboard(Key.W, (OnKeyEvent e) =>
        {
            Debug.Log("W key: Pressing with: " + e.Ctx.action);
        });
        InputManager.BindNewKeyboard(Key.W, (OnKeyEvent e) =>
        {
            Debug.Log("hello world with W key");
        });
        
        InputManager.BindNewMouse(KeyCode.Mouse0, (OnMouseEvent e) =>
        {
            Debug.Log("Left mouse button: Pressing with: " + e.Ctx.action);
        });
        InputManager.BindNewMouse(KeyCode.Mouse5, (OnMouseEvent e) =>
        {
            Debug.Log($"moving mouse at {e.Position}");
        });
        
    }

    private IEnumerator Temp()
    {
        yield return new WaitForFixedUpdate();
        // while (true)
        // {
        //     yield return new WaitForSeconds(1 / 20f);
        //     if(PlayerManager.LocalPlayer == null) continue;
        //     Camera.main.transform.position += new Vector3(2, 0, 0);
        //     PlayerManager.LocalPlayer.Position = Camera.main.transform.position;
        // }
    }
}

//!TODO CREATE WORLD WHEN NEEDED NOT AT START 
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
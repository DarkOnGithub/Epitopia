using System.Collections;
using UnityEngine;
using World.Blocks;
using World.WorldGeneration;

public class Main : MonoBehaviour
{
    public static Main instance;

    [SerializeField] public RuleTile ruleTile;

    private async void Start()
    {
        Seed.Initialize();
        instance = this;
        BlockRegistry.RegisterBlocks();
//await ServerCommand.CreateServer("Test");
        foreach (var v in Resources.Load<RuleTile>("Sprites/Blocks/DefaultRuleTile").m_TilingRules)
        {
            Debug.Log(string.Join(" ", v.m_Neighbors));
            Debug.Log(string.Join(" ", v.m_NeighborPositions));
        }

        StartCoroutine(Temp());
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
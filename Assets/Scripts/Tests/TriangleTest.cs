using System;
using System.Collections;
using System.Collections.Generic;
using Network.Packets;
using Network.Packets.Packets.Test;
using Unity.Netcode;
using UnityEngine;

namespace Tests
{
    public class TriangleTest: NetworkBehaviour
    {
        public static TriangleTest Instance;

        public void Awake()
        {
            Instance = this;
        }

        public  void T()
        {
            StartCoroutine(Temp());
        }
        IEnumerator Temp()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                Vector2 pos = Input.mousePosition;
                var packet = new MousePacketData()
                             {
                                 X = pos.x,
                                 Y = pos.y,
                                 Time = Time.realtimeSinceStartup * 1000
                             };
                MessageFactory.SendPacketToAll(packet);
            }
        } 
    }
}
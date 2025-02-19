using System.Collections;
using UnityEngine;
using World;

namespace Entities.AI
{
    public class AIFollower : AbstractAIState
    {
        public Vector2 Target { get; set; }
        public float Range { get; set; }
        
        public override PathFinder PathFinder { get; } 
        
        public AIFollower(AbstractWorld worldIn) : base(worldIn)
        {
            PathFinder = new PathFinder(worldIn);
        }

        public override bool ShouldExecute()
        {
            throw new System.NotImplementedException();
        }

        public override void CancelTask()
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerator UpdateTask()
        {
            throw new System.NotImplementedException();
        }

        public override void SetTask()
        {
            throw new System.NotImplementedException();
        }

        public override void SetPath()
        {
            throw new System.NotImplementedException();
        }


    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Entities.AI
{
    public abstract class AbstractAIState
    {
        public abstract PathFinder PathFinder { get; }
        protected Queue<Vector2> Path = new();
        protected AbstractWorld WorldIn;


        protected AbstractAIState(AbstractWorld worldIn)
        {
            WorldIn = worldIn;
        }

        public abstract bool ShouldExecute();
        public abstract void SetTask();
        public abstract IEnumerator UpdateTask();
        public abstract void CancelTask();

        public abstract void SetPath();


        public virtual void Idle()
        {
            
        }
        
    }
}
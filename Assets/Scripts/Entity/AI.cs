using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;
using World;

namespace Entities
{
    public class AI
    {
        protected readonly AbstractEntity Entity;
        protected readonly Transform Transform;
        protected readonly AbstractWorld WorldIn;
        protected List<Vector2Int> Path = new();
        
        public AI(AbstractEntity entity)
        {
	        
	        Entity = entity;
	        WorldIn = Entity.WorldIn;
	        Transform = Entity.Prefab.transform;
        }

        public void SetPath(List<Vector2Int> path)
        {
	        Path = path;
        }
        
		public void MoveTo(Vector2 destination)
		{
			Transform.position = Vector2.MoveTowards(
				Transform.position, 
				destination, 
				Entity.Speed * Time.deltaTime
				);
		}

		public void StepForward()
		{
			if(Path.Count == 0) return;
			MoveTo(Path[0]);
			if (Vector2.Distance(Transform.position, Path[0]) < 0.1f)
				Path.RemoveAt(0);
		}
    }   
}

using UnityEngine;
using UnityHFSM;
using World;

namespace Entities
{
    public abstract class AbstractEntity
    {
        public readonly AbstractWorld WorldIn;
        public readonly StateMachine StateMachine = new();
        public readonly AI AI;
        
        public float Speed = 15f;
        public readonly PathFinder PathFinder;
        public Vector2 Position;

        public readonly GameObject Prefab;
        public EntityBehaviour EntityBehaviour;
        
        protected AbstractEntity(AbstractWorld worldIn, string prefabPath)
        {
            WorldIn = worldIn;
            Prefab = GameObject.Instantiate(Resources.Load<GameObject>($"Prefabs/Entities/{prefabPath}"));
            Prefab.SetActive(false);
            EntityBehaviour = Prefab.GetComponent<EntityBehaviour>();
            EntityBehaviour.Init(this);

            PathFinder = new(worldIn);
            AI = new(this);

        }


        public void Spawn(Vector2 position)
        {
            Position = position;
            Prefab.transform.position = position;
            Prefab.SetActive(true);
        }
        
        
    }
}
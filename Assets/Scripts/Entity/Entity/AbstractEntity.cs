using System.Collections.Generic;
using Mono.CSharp;
using UnityEngine;
using Utils;
using World;
using StateMachine = UnityHFSM.StateMachine;

namespace Entities.Entity
{
    public abstract class AbstractEntity
    {
        public readonly string Name;
        public virtual List<EntityTags> Tags { get; } = new() { EntityTags.TagLess };
        protected readonly StateMachine StateMachine = new();
        protected readonly GameObject Prefab;
        protected Transform Transform => Prefab.transform;
        public Vector2 Position => Transform.position;
        
        public readonly EntityBehaviour Behaviour;
        
        public readonly AbstractWorld WorldIn;

        protected AbstractEntity(AbstractWorld worldIn, string name)
        {
            WorldIn = worldIn;
            Name = name;
            Prefab = GameObject.Instantiate(Resources.Load<GameObject>($"Prefabs/Entities/{name}"));
            Prefab.SetActive(false);
            Behaviour = Prefab.AddComponent<EntityBehaviour>();
            Behaviour.Init(this);
        }

        public void Spawn(Vector2 at)
        {
            Transform.position = at;
            Behaviour.targetPosition = Vector2Int.RoundToInt(at);
            Prefab.SetActive(true);
        }
        
        public void MoveTo(Vector2Int position)
        {
            Behaviour.SetTarget(position);
        }

    }
}
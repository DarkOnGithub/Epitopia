using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;
using World;

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
        
        public AbstractWorld WorldIn;

        protected AbstractEntity(AbstractWorld worldIn, string name)
        {
            WorldIn = worldIn;
            Name = name;
            Prefab = Resources.Load<GameObject>($"Prefabs/Entities/{name}");
        }
        
        





    }
}
using System;
using System.Collections.Generic;
using Entities.Entity;
using UnityEngine;

namespace Entities
{
    public class EntityManager : MonoBehaviour
    {
        private EntityTagsUnion _entityTags;


        public void Awake()
        {
            _entityTags = new EntityTagsUnion(Enum.GetValues(typeof(EntityTags)) as EntityTags[]);            
        }


        public void SpawnEntity<T>() where T : AbstractEntity, new()
        {
            var entity = new T();
            _entityTags.AddEntity(entity);
        }
        
    }
}